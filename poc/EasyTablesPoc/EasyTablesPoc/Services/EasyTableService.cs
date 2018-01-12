using EasyTablesPoc.Helpers;
using EasyTablesPoc.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyTablesPoc.Services
{
    public class EasyTableService<T> where T : EasyTableBase
    {
        protected MobileServiceClient _client;
        protected IMobileServiceSyncTable<T> _table;

        public event Action<T, T> OnResolveConflict;

        public EasyTableService()
        {
            _client = MobileConfiguration.Instance.Client;
            _table = _client.GetSyncTable<T>();
        }
        
        public async Task SyncAsync()
        {
            await MobileConfiguration.Instance.InitializeAsync();
            if (InternetConnection.Instance.IsInternetAvailable)
            {
                try
                {
                    await _client.SyncContext.PushAsync();
                    await _table.PullAsync($"all{typeof(T).Name}", _table.CreateQuery());
                }
                catch (MobileServicePushFailedException ex) when (ex.PushResult != null)
                {
                    foreach (var error in ex.PushResult.Errors)
                    {
                        await ResolveErrorAsync(error);
                        NotifyConflictResolution(error);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public virtual async Task<IEnumerable<T>> ReadAsync()
        {
            await SyncAsync();
            return await _table.ReadAsync();
        }

        public virtual async Task<T> LookupAsync(string id)
        {
            await SyncAsync();
            return await _table.LookupAsync(id);
        }

        public virtual async Task AddOrUpdateAsync(T item)
        {
            await SyncAsync();

            if (string.IsNullOrEmpty(item.Id))
            {
                await _table.InsertAsync(item);
            }
            else
            {
                await _table.UpdateAsync(item);
            }

            await SyncItemAsync(item.Id);
        }

        public virtual async Task DeleteAsync(T item)
        {
            await SyncAsync();
            await _table.DeleteAsync(item);
            await SyncItemAsync(item.Id);
        }

        private async Task SyncItemAsync(string itemId)
        {
            if (!InternetConnection.Instance.IsInternetAvailable)
                return;

            try
            {
                await _client.SyncContext.PushAsync();
                await _table.PullAsync($"sync{typeof(T).Name}" + itemId, _table.Where(r => r.Id == itemId));
            }
            catch (MobileServicePushFailedException ex) when (ex.PushResult != null)
            {
                foreach (var error in ex.PushResult.Errors)
                {
                    await ResolveErrorAsync(error);
                    NotifyConflictResolution(error);
                }
            }
            catch (Exception)
            {
            }
        }

        protected virtual async Task ResolveErrorAsync(MobileServiceTableOperationError error)
        {
            if (error.Result == null || error.Item == null)
            {
                return;
            }

            //default, server win
            await error.CancelAndUpdateItemAsync(error.Result);
        }

        private void NotifyConflictResolution(MobileServiceTableOperationError error)
        {
            var serverItem = error.Result.ToObject<T>();
            var localItem = error.Item.ToObject<T>();

            OnResolveConflict?.Invoke(serverItem, localItem);
        }
    }
}
