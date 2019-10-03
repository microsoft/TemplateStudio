namespace Param_RootNamespace.Core.Services
{
    public interface IIdentityService
    {
        Task<string> GetAccessTokenForGraphAsync();
//{[{

        Task<string> GetAccessTokenForWebApiAsync();
//}]}
    }
}