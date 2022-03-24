namespace Param_RootNamespace.WebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
//{[{
            services.ProtectWebApiWithJwtBearer(Configuration);
//}]}
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
//{[{
            app.UseAuthentication();
            app.UseAuthorization();
//}]}
        }
    }
}