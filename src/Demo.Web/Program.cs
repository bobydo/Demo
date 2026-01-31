using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Demo.Web.Models; // For Trip_DTO
using Demo.Web.Services; // For TripsService (if you put TripsService in Services folder)

namespace Demo.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Root component
            builder.RootComponents.Add<App>("#app");

            // Configure HttpClient for calling server API
            builder.Services.AddScoped(sp =>
                new HttpClient { BaseAddress = new Uri("http://localhost:5000/") });

            string url = "http://localhost:5001/railcar-trips";
            Console.WriteLine($"Weburl UI: {url}");

            // Automatically open browser (only on local dev)
            if (OperatingSystem.IsWindows() || OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
            {
                try
                {
                    // .NET 6+ cross-platform way
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to open browser: {ex.Message}");
                }
            }

            // Register TripsService
            builder.Services.AddScoped<TripsService>();

            await builder.Build().RunAsync();
        }
    }
}
