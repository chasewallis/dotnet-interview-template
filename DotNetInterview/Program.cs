using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetInterview
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            // NOTE: I decided to go with Transient here. Since this is not an ASP.Net app, Scope  doesn't make sense,
            // Singleton is an option, but I tend to prefer transient unless there is a reason to use singleton. 
            serviceCollection.AddTransient<ISoftwareReporter, SoftwareReporter>();
            serviceCollection.AddTransient<IApiService, ApiService>();
            serviceCollection.AddTransient<ISoftwareReporter, SoftwareReporter>();
            
            var container = serviceCollection.BuildServiceProvider();

            var reporter = container.GetService<ISoftwareReporter>();

            if (reporter != null)
            {
                await reporter.ReportSoftwareInstallationStatus("Syncro");
            }
            else
            {
                throw new ArgumentNullException(nameof(reporter));
            }

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
