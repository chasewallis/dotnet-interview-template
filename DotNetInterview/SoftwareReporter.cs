using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetInterview
{
    public interface ISoftwareReporter
    {
        Task ReportSoftwareInstallationStatus(string softwareName);
    }

    public class SoftwareReporter : ISoftwareReporter
    {
        private readonly IRegistryService _registryService;
        private readonly IApiService _apiService;
        private readonly ILogger<SoftwareReporter> _logger;

        public SoftwareReporter(IRegistryService registryService, IApiService apiService, ILogger<SoftwareReporter> logger)
        {
            _registryService = registryService;
            _apiService = apiService;
            _logger = logger;
        }

        public async Task ReportSoftwareInstallationStatus(string softwareName)
        {
            if (string.IsNullOrWhiteSpace(softwareName))
            {
                throw new ArgumentNullException(nameof(softwareName));
            }            
            var isInstalled = _registryService.CheckIfInstalled(softwareName);
            await _apiService.SendInstalledSoftware(softwareName, isInstalled);
        }
    }
}
