using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // TODO: Implement this method to check if the given software name
            // can be found as an installed application in the Windows registry.

            // The search should be case insensitive and should
            // include partial matches.
            
            // NOTE : If this full path is possible, opening the subkeys one at a time is not necessary.
            // Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\")

            return GetInstalledApplications()
                .Any(item => softwareName.Equals(item, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<string> GetInstalledApplications()
        {
            // Computer\HKEY_LOCAL_MACHINE\SOFTWARE\            Microsoft\Windows\CurrentVersion\Uninstall\
            // Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\
            
            // I think I need to open each subkey individually and not go direct to it with the full path.
            using (var software = Registry.LocalMachine.OpenSubKey("SOFTWARE"))
            {
                var uninstallNode = GetUninstallNodeSoftware(software);
                using(var wow64Node = software.OpenSubKey("Wow6432Node"))
                {
                    var uninstallNode6432 = GetUninstallNodeSoftware(wow64Node);
                }
            }
        }

        private RegistryKey GetUninstallNodeSoftware(RegistryKey software)
        {
            using(var microsoft = software.OpenSubKey("Microsoft"))
            {
                    
            }
        }
        
    }
}
