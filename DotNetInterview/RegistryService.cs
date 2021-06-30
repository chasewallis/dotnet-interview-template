using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using StringExtensions;

namespace DotNetInterview
{
    public interface IRegistryService
    {
        bool CheckIfInstalled(string softwareName);
    }

    public class RegistryService : IRegistryService
    {
        private ILogger<RegistryService> _logger;

        public RegistryService(ILogger<RegistryService> logger)
        {
            _logger = logger;
        }

        public bool CheckIfInstalled(string softwareName)
        {
            // The search should be case insensitive and should
            // include partial matches.
            
            if (softwareName == null)
            {
                throw new ArgumentNullException(nameof(softwareName));
            }
            
            return GetInstalledApplications().Any(item => item.Contains(softwareName, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<string> GetInstalledApplications()
        {
            var uninstallKeyPath =
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
            var wow64UninstallKeyPath =
                @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";

            foreach (var applicationName in GetApplicationNames(uninstallKeyPath))
            {
                yield return applicationName;
            }

            foreach (var applicationName in GetApplicationNames(wow64UninstallKeyPath))
            {
                yield return applicationName;
            }
        }

        private static IEnumerable<string> GetApplicationNames(string uninstallKeyPath)
        {
            using (var uninstallKey = Registry.LocalMachine.OpenSubKey(uninstallKeyPath))
            {
                if (uninstallKey != null)
                {
                    foreach (var guid in uninstallKey.GetSubKeyNames())
                    {
                        using (var guidKey = uninstallKey.OpenSubKey(guid))
                        {
                            if (guidKey != null)
                            {
                                var val = guidKey.GetValue("DisplayName");
                                if (val is String appName)
                                {
                                    yield return appName;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
