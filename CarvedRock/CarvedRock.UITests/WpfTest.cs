using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;

namespace CarvedRock.UITests
{
    [TestClass]
    public class WpfTest
    {
        [TestMethod]
        public void MasterDetail()
        {
            var capabilities = new AppiumOptions();
            capabilities.App = @"C:\source\AppiumFundamentals\CarvedRock\CarvedRock.Wpf\bin\Debug\CarvedRock.Wpf.exe";
            capabilities.PlatformName = "Windows";
            capabilities.DeviceName = "WindowsPC";
            capabilities.AutomationName = "Windows";

            System.Environment.SetEnvironmentVariable("APPIUM_HOME", @"C:\Users\vries\.appium\node_modules\appium-windows-driver");

            var _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().WithLogFile(new FileInfo(@"c:\tmp\appiumlogfile.txt")).Build();
            _appiumLocalService.Start(); ;
            using var driver = new WindowsDriver(_appiumLocalService, capabilities);

            // tap on second item
            var el1 = driver.FindElement(MobileBy.AccessibilityId("Second item"));
            el1.Click();

            var el2 = driver.FindElement(MobileBy.AccessibilityId("txtItemText"));
            Assert.IsTrue(el2.Text == "Second item");

            var okButton = driver.FindElement(MobileBy.AccessibilityId("ok"));
            okButton.Click();

            var el3 = driver.FindElement(MobileBy.AccessibilityId("Fourth item"));
            Assert.IsTrue(el3 != null);
        }

    }
}
