﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.Core.Services;

public class MicrosoftGraphService : IMicrosoftGraphService
{
    /*
    For more information about Get-User Service, refer to the following documentation
    https://docs.microsoft.com/graph/api/user-get?view=graph-rest-1.0
    You can test calls to the Microsoft Graph with the Microsoft Graph Explorer
    https://developer.microsoft.com/graph/graph-explorer
    */

    private const string _apiServiceMe = "me/";
    private const string _apiServiceMePhoto = "me/photo/$value";
    private readonly HttpClient _client;

    public MicrosoftGraphService(IHttpClientFactory client)
    {
        _client = client.CreateClient("msgraph");
    }

    public async Task<User> GetUserInfoAsync(string accessToken)
    {
        User user = null;
        var httpContent = await GetDataAsync($"{_apiServiceMe}", accessToken);
        if (httpContent != null)
        {
            var userData = await httpContent.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(userData))
            {
                user = JsonConvert.DeserializeObject<User>(userData);
            }
        }

        return user;
    }

    public async Task<string> GetUserPhoto(string accessToken)
    {
        var httpContent = await GetDataAsync($"{_apiServiceMePhoto}", accessToken);

        if (httpContent == null)
        {
            return string.Empty;
        }

        var stream = await httpContent.ReadAsStreamAsync();
        return stream.ToBase64String();
    }

    private async Task<HttpContent> GetDataAsync(string url, string accessToken)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return response.Content;
            }
            else
            {
                // TODO: Please handle other status codes as appropriate to your scenario
            }
        }
        catch (HttpRequestException)
        {
            // TODO: The request failed due to an underlying issue such as
            // network connectivity, DNS failure, server certificate validation or timeout.
            // Please handle this exception as appropriate to your scenario
        }
        catch (Exception)
        {
            // TODO: This call can fail please handle exceptions as appropriate to your scenario
        }

        return null;
    }
}
