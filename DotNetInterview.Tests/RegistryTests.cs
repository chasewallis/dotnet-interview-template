using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DotNetInterview.Tests
{
    /// <summary>
    ///  This is technically an integration test, but I felt it necessary to make some sort of test to make sure the
    /// registration search was working correctly
    /// </summary>
    [TestClass]
    public class RegistryTests
    {
        private RegistryService _registryMock;

        [TestInitialize]
        public void Init()
        {
            var loggerMock = new Mock<ILogger<RegistryService>>();
            _registryMock = new RegistryService(loggerMock.Object);
        }
        
        [TestMethod]
        public void CheckIfInstalled_Name_of_nonexistent_app()
        {
            var isInstalled = _registryMock.CheckIfInstalled("Just a string that is guaranteed not to be an installed application -qwerasdfzxcv");
            Assert.IsFalse(isInstalled);
        }
        
        [TestMethod]
        public void CheckIfInstalled_Name_of_probably_existent_app()
        {
            // Technically, this isn't guaranteed, it is very machine specific, so it really isn't a good test. But I
            // felt I needed something
            var isInstalled = _registryMock.CheckIfInstalled("microsoft");
            Assert.IsTrue(isInstalled);
        }
        
        [TestMethod]
        public void CheckIfInstalled_Name_of_probably_existent_app_ignore_case()
        {
            // Technically, this isn't guaranteed, it is very machine specific, so it really isn't a good test. But I
            // felt I needed something
            var isInstalled = _registryMock.CheckIfInstalled("MiCrOsOfT");
            Assert.IsTrue(isInstalled);
        }
    }
}