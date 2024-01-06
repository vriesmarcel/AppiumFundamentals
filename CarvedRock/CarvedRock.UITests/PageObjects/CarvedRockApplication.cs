using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using System;
using System.IO;

namespace CarvedRock.UITests.PageObjects
{
    internal class CarvedRockApplication
    {
        private AndroidDriver _driver = null;
        public CarvedRockApplication()
        {
        }
        public AndroidDriver Driver
        { get { return _driver; } }
        internal MainScreen StartApplication()
        {
            _driver = CreateApplication();
            return new MainScreen(this);
        }

        internal void CloseApplication()
        {
            if (_driver != null)
                _driver.Dispose();
        }
        private AndroidDriver CreateApplication()
        {
            Environment.SetEnvironmentVariable("ANDROID_HOME", @"C:\Program Files (x86)\Android\android-sdk");
            Environment.SetEnvironmentVariable("JAVA_HOME", @"C:\Program Files\Android\jdk\jdk-8.0.302.8-hotspot\jdk8u302-b08");
            Environment.SetEnvironmentVariable("APPIUM_HOME", @"C:\Users\vries\.appium\node_modules\appium-uiautomator2-driver");

            var capabilities = new AppiumOptions();
            // automatic start of the emulator if not running
            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.Avd, "demo_device");
            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AvdArgs, "-no-boot-anim -no-snapshot-load");
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            // connecting to a device or emulator
            capabilities.DeviceName = "sdk_gphone64_x86_64";
            capabilities.AutomationName = "UiAutomator2";
            // specifyig which app we want to install and launch
            var currentPath = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current path: {currentPath}");
            var packagePath = Path.Combine(currentPath, @"..\..\..\AppsToTest\com.fluentbytes.carvedrock-x86_64.apk");
            packagePath = Path.GetFullPath(packagePath);
            Console.WriteLine($"Package path: {packagePath}");
            capabilities.App = packagePath;

            var _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().WithLogFile(new FileInfo(@"c:\tmp\appiumlogfile.txt")).Build();
            _appiumLocalService.Start(); ;
            var driver = new AndroidDriver(_appiumLocalService, capabilities);
            return driver;
        }
    }
}