```csharp
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Application_Start
{
    public class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithSTAThread")]
        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("No input arguments provided.");
                return;
            }

            try
            {
                var webHostBuilder = CreateWebHostBuilder(args);
                if (webHostBuilder == null)
                {
                    throw new InvalidOperationException("Failed to create the web host builder.");
                }
                webHostBuilder.Build().Run();
            }
            catch (TypeLoadException ex) when (!IsStartupClassFound(ex))
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        private static bool IsStartupClassFound(Exception ex)
        {
            return ex is TypeLoadException || ex.InnerException is TypeLoadException;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .CaptureStartupErrors(true)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                });
    }
}
```