namespace Param_RootNamespace.WebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
//{[{
            services.ProtectWebApiWithJwtBearer(Configuration);
//}]}
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpsRedirection();
//{[{
            app.UseAuthentication();
//}]}
        }
    }
}