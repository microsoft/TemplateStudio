namespace Param_RootNamespace.WebApi.Models
{
    public class AuthenticationSettings
    {
        public string ClientId { get; set; }

        public string TenantId { get; set; }

        public string Audience { get; set; }

        public string Scope { get; set; }
    }
}