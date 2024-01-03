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
            AndroidDriver driver = CreateApplication();

            // Create new Category item first
            var categoryName = "New category from automation";
            CreateNewCategory(driver, categoryName);
            CreateNewItemInList(driver,categoryName);

            WaitForProgressBar(driver);

            AppiumElement elementfound = WaitForItemInview(driver);

            Assert.IsTrue(elementfound != null);

            driver.Dispose();
        }


        [TestMethod]
        public void AddNewItem()
        {
            AndroidDriver driver = CreateApplication();

            CreateNewItemInList(driver);

            WaitForProgressBar(driver);

            AppiumElement elementfound = WaitForItemInview(driver);

            Assert.IsTrue(elementfound != null);

            driver.Dispose();

        }

        [TestMethod]
        public void MasterDetail()
        {
            AndroidDriver driver = CreateApplication();

            // tap on second item
            var el1 = driver.FindElement(MobileBy.AccessibilityId("Second item"));
            el1.Click();

            var el2 = driver.FindElement(MobileBy.AccessibilityId("ItemText"));
            Assert.IsTrue(el2.Text == "Second item");

            driver.PressKeyCode(AndroidKeyCode.Back);

            var el3 = driver.FindElement(MobileBy.AccessibilityId("Fourth item"));
            Assert.IsTrue(el3 != null);

            driver.Dispose();
        }


        private static AppiumElement WaitForItemInview(AndroidDriver driver)
        {
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

            var wait = new DefaultWait<AndroidDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(1000)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            AppiumElement elementfound = null;

            elementfound = wait.Until(d =>
            {
                FlickUp(driver, listview);
                return FindElementAction();
            });
            return elementfound;
        }

        private static void FlickUp(AndroidDriver driver, AppiumElement listview)
        {
            var input = new PointerInputDevice(PointerKind.Touch);
            ActionSequence FlickUp = new ActionSequence(input);
            FlickUp.AddAction(input.CreatePointerMove(listview, 0, 0, TimeSpan.Zero));
            FlickUp.AddAction(input.CreatePointerDown(MouseButton.Left));

            FlickUp.AddAction(input.CreatePointerMove(listview, 0, -600, TimeSpan.FromMilliseconds(200)));
            FlickUp.AddAction(input.CreatePointerUp(MouseButton.Left));
            driver.PerformActions(new List<ActionSequence>() { FlickUp });
        }

        private static void WaitForProgressBar(AndroidDriver driver)
        {
            //wait for progress bar to disapear
            var wait = new DefaultWait<AndroidDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            wait.Until(d => d.FindElement(MobileBy.AccessibilityId("Second item")));
        }

        private static void CreateNewItemInList(AndroidDriver driver, string category = null)
        {
            var el1 = driver.FindElement(MobileBy.AccessibilityId("Add"));
            el1.Click();

            //create new item 
            var elItemText = driver.FindElement(MobileBy.AccessibilityId("ItemText"));
            elItemText.Clear();
            elItemText.SendKeys("This is a new Item");

            var elItemDetail = driver.FindElement(MobileBy.AccessibilityId("ItemDescription"));
            elItemDetail.Clear();
            elItemDetail.SendKeys("These are the details");
            //only select category when needed
            if (!string.IsNullOrEmpty(category))
            {
                var elItemCategory = driver.FindElement(MobileBy.AccessibilityId("ItemCategory_Container"));
                elItemCategory.Click();

                var picker = driver.FindElement(By.Id("android:id/select_dialog_listview"));
                var categoryListItems = picker.FindElements(By.ClassName("android.widget.TextView"));
                foreach (var categoryElement in categoryListItems)
                {
                    if (categoryElement.Text == category)
                        categoryElement.Click();
                }
            }

            var elSave = driver.FindElement(MobileBy.AccessibilityId("Save"));
            elSave.Click();
        }

        private static void CreateNewCategory(AndroidDriver driver, string category)
        {
            var categoryButton = driver.FindElement(MobileBy.AccessibilityId("AddCategory"));
            categoryButton.Click();

            // fill out the form for a new category
            var categoryName = driver.FindElement(MobileBy.AccessibilityId("categoryName"));
            categoryName.Clear();
            categoryName.SendKeys(category);

            //save category
            var saveCategory = driver.FindElement(MobileBy.AccessibilityId("Save"));
            saveCategory.Click();
        }

        private static AndroidDriver CreateApplication()
        {
            System.Environment.SetEnvironmentVariable("ANDROID_HOME", @"C:\Program Files (x86)\Android\android-sdk");
            System.Environment.SetEnvironmentVariable("JAVA_HOME", @"C:\Program Files\Android\jdk\jdk-8.0.302.8-hotspot\jdk8u302-b08");
            System.Environment.SetEnvironmentVariable("APPIUM_HOME", @"C:\Users\vries\.appium\node_modules\appium-uiautomator2-driver");

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
