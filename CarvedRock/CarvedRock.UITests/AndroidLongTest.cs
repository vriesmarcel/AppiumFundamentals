using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace CarvedRock.UITests
{
    [TestClass]
    public class AndroidLongTest
    {
        [TestMethod]
        public void AddNewItemWithNewCategory()
        {
            System.Environment.SetEnvironmentVariable("ANDROID_HOME", @"C:\Program Files (x86)\Android\android-sdk");
            System.Environment.SetEnvironmentVariable("JAVA_HOME", @"C:\Program Files\Android\jdk\jdk-8.0.302.8-hotspot\jdk8u302-b08");

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
            var packagePath = Path.Combine(currentPath, @"..\..\..\AppsToTest\com.fluentbytes.carvedrock-Signed.apk");
            packagePath = Path.GetFullPath(packagePath);
            Console.WriteLine($"Package path: {packagePath}");
            capabilities.App = packagePath;
            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, "com.fluentbytes.carvedrock");
            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, "crc641782d5af3c9cf50a.MainActivity");

            //var _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().WithLogFile(new FileInfo(@"c:\tmp\appiumlogfile.txt")).Build();
            //_appiumLocalService.Start(); ;
            //var driver = new AndroidDriver(_appiumLocalService, capabilities);
           var driver = new AndroidDriver(new Uri("http://127.0.0.1:4723/"), capabilities);
            // Create new Category item first
            var categoryButton = driver.FindElement(MobileBy.AccessibilityId("AddCategory"));
            categoryButton.Click();

            // fill out the form for a new category
            var categoryName = driver.FindElement(MobileBy.AccessibilityId("categoryName"));
            categoryName.Clear();
            categoryName.SendKeys("New category from automation");

            //save category
            var saveCategory = driver.FindElement(MobileBy.AccessibilityId("Save"));
            saveCategory.Click();

            var el1 = driver.FindElement(MobileBy.AccessibilityId("Add"));
            el1.Click();

            var elItemText = driver.FindElement(MobileBy.AccessibilityId("ItemText"));
            elItemText.Clear();
            elItemText.SendKeys("This is a new Item");

            var elItemDetail = driver.FindElement(MobileBy.AccessibilityId("ItemDescription"));
            elItemDetail.Clear();
            elItemDetail.SendKeys("These are the details");

            var elItemCategory = driver.FindElement(MobileBy.AccessibilityId("ItemCategory_Container"));
            elItemCategory.Click();

            var picker = driver.FindElement(By.Id("android:id/contentPanel"));
            var categoryListItems = picker.FindElements(By.ClassName("android.widget.TextView"));
            foreach(var categoryElement in categoryListItems)
            {
                if (categoryElement.Text == "New category from automation")
                    categoryElement.Click();
            }
           

            var elSave = driver.FindElement(MobileBy.AccessibilityId("Save"));
            elSave.Click();

            //wait for progress bar to disapear
            var wait = new DefaultWait<AndroidDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            wait.Until(d => d.FindElement(MobileBy.AccessibilityId("Second item")));


            var listview = driver.FindElement(MobileBy.AccessibilityId("ItemsListView"));

            //now use wait to scroll untill we find item

            Func<AppiumElement> FindElementAction = () =>
            {
                // find all text views
                // check if the text matches
                var elements = driver.FindElements(MobileBy.ClassName("android.widget.TextView"));
                foreach (var textView in elements)
                {
                    if (textView.Text == "This is a new Item")
                        return textView;
                }
                return null;
            };

            wait = new DefaultWait<AndroidDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(1000)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            AppiumElement elementfound = null;

            elementfound = wait.Until(d =>
            {
                var input = new PointerInputDevice(PointerKind.Touch);
                ActionSequence FlickUp = new ActionSequence(input);
                FlickUp.AddAction(input.CreatePointerMove(listview, 0, 0, TimeSpan.Zero));
                FlickUp.AddAction(input.CreatePointerDown(MouseButton.Left));

                FlickUp.AddAction(input.CreatePointerMove(listview, 0, -600, TimeSpan.FromMilliseconds(200)));
                FlickUp.AddAction(input.CreatePointerUp(MouseButton.Left));
                driver.PerformActions(new List<ActionSequence>() { FlickUp });
                return FindElementAction();
            });

            Assert.IsTrue(elementfound != null);

            driver.CloseApp();
        }

        [TestMethod]
        public void AddNewItem()
        {
            System.Environment.SetEnvironmentVariable("ANDROID_HOME", @"C:\Program Files (x86)\Android\android-sdk");
            System.Environment.SetEnvironmentVariable("JAVA_HOME", @"C:\Program Files\Android\jdk\microsoft_dist_openjdk_1.8.0.25\bin");

            var capabilities = new AppiumOptions();
            // automatic start of the emulator if not running
            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.Avd, "demo_device");
            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AvdArgs, "-no-boot-anim -no-snapshot-load");
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            // connecting to a device or emulator
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.DeviceName, "2471736c36037ece");
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.AutomationName, "UiAutomator2");
            // specifyig which app we want to install and launch
            var currentPath = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current path: {currentPath}");
            var packagePath = Path.Combine(currentPath, @"..\..\..\AppsToTest\com.fluentbytes.carvedrock-x86.apk");
            packagePath = Path.GetFullPath(packagePath);
            Console.WriteLine($"Package path: {packagePath}");
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.App, packagePath);

            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, "com.fluentbytes.carvedrock");
            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, "crc641782d5af3c9cf50a.MainActivity");

            var _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            _appiumLocalService.Start(); ;
            var driver = new AndroidDriver(_appiumLocalService, capabilities);

            var el1 = driver.FindElement(MobileBy.AccessibilityId("Add"));
            el1.Click();

            var elItemText = driver.FindElement(MobileBy.AccessibilityId("ItemText"));
            elItemText.Clear();
            elItemText.SendKeys("This is a new Item");

            var elItemDetail = driver.FindElement(MobileBy.AccessibilityId("ItemDescription"));
            elItemDetail.Clear();
            elItemDetail.SendKeys("These are the details");

            var elSave = driver.FindElement(MobileBy.AccessibilityId("Save"));
            elSave.Click();

            //wait for progress bar to disapear
            var wait = new DefaultWait<AndroidDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            wait.Until(d => d.FindElement(MobileBy.AccessibilityId("Second item")));


            var listview = driver.FindElement(MobileBy.AccessibilityId("ItemsListView"));

            //now use wait to scroll untill we find item

            Func<AppiumElement> FindElementAction = () =>
            {
                // find all text views
                // check if the text matches
                var elements = driver.FindElements(MobileBy.ClassName("android.widget.TextView"));
                foreach (var textView in elements)
                {
                    if (textView.Text == "This is a new Item")
                        return textView;
                }
                return null;
            };

            wait = new DefaultWait<AndroidDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(1000)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            AppiumElement elementfound = null;

            elementfound = wait.Until(d =>
            {
                var input = new PointerInputDevice(PointerKind.Touch);
                ActionSequence FlickUp = new ActionSequence(input);
                FlickUp.AddAction(input.CreatePointerMove(listview, 0, 0, TimeSpan.Zero));
                FlickUp.AddAction(input.CreatePointerDown(MouseButton.Left));

                FlickUp.AddAction(input.CreatePointerMove(listview, 0, -600, TimeSpan.FromMilliseconds(200)));
                FlickUp.AddAction(input.CreatePointerUp(MouseButton.Left));
                driver.PerformActions(new List<ActionSequence>() { FlickUp });
                return FindElementAction();
            });

            Assert.IsTrue(elementfound != null);

            driver.CloseApp();

        }

        [TestMethod]
        public void MasterDetail()
        {
            System.Environment.SetEnvironmentVariable("ANDROID_HOME", @"C:\Program Files (x86)\Android\android-sdk");
            System.Environment.SetEnvironmentVariable("JAVA_HOME", @"C:\Program Files\Android\jdk\microsoft_dist_openjdk_1.8.0.25\bin");

            var capabilities = new AppiumOptions();
            // automatic start of the emulator if not running
            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.Avd, "demo_device");
            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AvdArgs, "-no-boot-anim -no-snapshot-load");
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            // connecting to a device or emulator
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.DeviceName, "2471736c36037ece");
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.AutomationName, "UiAutomator2");
            // specifyig which app we want to install and launch
            var currentPath = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current path: {currentPath}");
            var packagePath = Path.Combine(currentPath, @"..\..\..\AppsToTest\com.fluentbytes.carvedrock-x86.apk");
            packagePath = Path.GetFullPath(packagePath);
            Console.WriteLine($"Package path: {packagePath}");
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.App, packagePath);

            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, "com.fluentbytes.carvedrock");
            capabilities.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, "crc641782d5af3c9cf50a.MainActivity");

            var _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            _appiumLocalService.Start(); ;
            var driver = new AndroidDriver(_appiumLocalService, capabilities);
            // tap on second item
            var el1 = driver.FindElement(MobileBy.AccessibilityId("Second item"));
            el1.Click();

            var el2 = driver.FindElement(MobileBy.AccessibilityId("ItemText"));
            Assert.IsTrue(el2.Text == "Second item");

            driver.PressKeyCode(AndroidKeyCode.Back);

            var el3 = driver.FindElement(MobileBy.AccessibilityId("Fourth item"));
            Assert.IsTrue(el3 != null);

            driver.CloseApp();
        }
    }
}
