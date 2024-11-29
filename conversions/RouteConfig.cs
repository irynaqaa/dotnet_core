```csharp
using Microsoft.AspNetCore.Builder;
using System;

namespace WebApiApplication.Configurations
{
    public class RouteConfigurator
    {
        public void ConfigureRoutes(IRouteBuilder routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            var routePatterns = new[]
            {
                new { Name = "default", Pattern = "{controller=Home}/{action=Index}/{id?}" },
                new { Name = "api", Pattern = "api/{controller}/{action}/{id?}", Defaults = new { controller = "Values", action = "Get" } },
                new { Name = "values", Pattern = "api/values/{id?}", Defaults = new { controller = "Values", action = "Get" } }
            };

            foreach (var routePattern in routePatterns)
            {
                if (string.IsNullOrWhiteSpace(routePattern.Pattern))
                {
                    throw new ArgumentException("Route pattern cannot be null or empty.", nameof(routePattern));
                }

                var existingRoute = routes.Routes.FirstOrDefault(r => r.Name == routePattern.Name);
                if (existingRoute != null)
                {
                    throw new InvalidOperationException($"A route with the name '{routePattern.Name}' already exists.");
                }

                routes.MapControllerRoute(
                    name: routePattern.Name,
                    pattern: routePattern.Pattern,
                    defaults: routePattern.Defaults);
            }
        }
    }
}
```