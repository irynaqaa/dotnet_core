```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiConfig
{
    public class WebApiConfigurator
    {
        public void Register(IServiceCollection services, IApplicationBuilder app)
        {
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```