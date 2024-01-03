using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvedRock.UITests
{
    [TestClass]
    public class WinFormsTest
    {
        [TestMethod]
        public void MasterDetail()
        {
            var capabilities = new AppiumOptions();
            capabilities.App = @"C:\source\AppiumFundamentals\CarvedRock\CarvedRock.winforms\bin\Debug\CarvedRock.exe";
            capabilities.PlatformName = "Windows";
            capabilities.DeviceName = "WindowsPC";
            capabilities.AutomationName = "Windows";

            System.Environment.SetEnvironmentVariable("APPIUM_HOME", @"C:\Users\vries\.appium\node_modules\appium-windows-driver");

            var _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().WithLogFile(new FileInfo(@"c:\tmp\appiumlogfile.txt")).Build();
            _appiumLocalService.Start(); ;
            using var driver = new WindowsDriver(_appiumLocalService, capabilities);

            // tap on second item
            var el1 = driver.FindElement(MobileBy.Name("Second item"));
            el1.Click();

            var el2 = driver.FindElement(MobileBy.AccessibilityId("lblItemText"));
            // it is not possible to read the text of the label control, only from edit controls
            // Assert.IsTrue(el2.Text == "Second item");

            var okButton = driver.FindElement(MobileBy.AccessibilityId("button1"));
            okButton.Click();

            var el3 = driver.FindElement(MobileBy.Name("Fourth item"));
            Assert.IsTrue(el3 != null);
        }
    }
}
