﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
//using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarvedRock.UITests
{
    [TestClass]
    public class WindowsUWPLongTest
    {
        [TestMethod]
        public void AddNewItemWithNewCategory()
        {
            var capabilities = new AppiumOptions();
            capabilities.App = "8b831c56-bc54-4a8b-af94-a448f80118e7_sezxftbtgh66j!App";
            capabilities.PlatformName = "Windows";
            capabilities.DeviceName = "WindowsPC";

            var _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            _appiumLocalService.Start();
            var driver = new WindowsDriver(_appiumLocalService, capabilities);

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

            var elItemCategory = driver.FindElement(MobileBy.AccessibilityId("ItemCategory"));
            elItemCategory.Click();

            var categoryListItem = elItemCategory.FindElement(By.Name("New category from automation"));
            categoryListItem.Click();

            var elSave = driver.FindElement(MobileBy.AccessibilityId("Save"));
            elSave.Click();
            elSave.ClearCache();

            //wait for progress bar to disapear
            var wait = new DefaultWait<WindowsDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(d => d.FindElement(MobileBy.AccessibilityId("Second item")));

            var listview = driver.FindElement(MobileBy.AccessibilityId("ItemsListView"));

            //now use wait to scroll untill we find item
            wait = new DefaultWait<WindowsDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            var elementfound = wait.Until(d =>
            {
                var input = new PointerInputDevice(PointerKind.Touch);
                ActionSequence FlickUp = new ActionSequence(input);
                FlickUp.AddAction(input.CreatePointerMove(listview, 0, 0, TimeSpan.Zero));
                FlickUp.AddAction(input.CreatePointerDown(MouseButton.Left));
                FlickUp.AddAction(input.CreatePointerMove(listview, 0, -300, TimeSpan.FromMilliseconds(200)));
                FlickUp.AddAction(input.CreatePointerUp(MouseButton.Left));
                driver.PerformActions(new List<ActionSequence>() { FlickUp });

                return d.FindElement(MobileBy.AccessibilityId("This is a new Item"));
            });

            Assert.IsTrue(elementfound != null);

            driver.CloseApp();
        }

        [TestMethod]
        public void AddNewItem()
        {
            var capabilities = new AppiumOptions();
            capabilities.App = "8b831c56-bc54-4a8b-af94-a448f80118e7_sezxftbtgh66j!App";
            capabilities.PlatformName= "Windows";
            capabilities.DeviceName = "WindowsPC";
            capabilities.AutomationName = "Windows";
            System.Environment.SetEnvironmentVariable("APPIUM_BINARY_PATH", @"C:\Users\vries\.appium");

            //var _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            //_appiumLocalService.Start();
            //var driver = new WindowsDriver(_appiumLocalService, capabilities);
            var driver = new WindowsDriver(new Uri("http://127.0.0.1:4723/"), capabilities);

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
            var wait = new DefaultWait<WindowsDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(d => d.FindElement(MobileBy.AccessibilityId("Second item")));

            var listview = driver.FindElement(MobileBy.AccessibilityId("ItemsListView"));

            //now use wait to scroll untill we find item
            wait = new DefaultWait<WindowsDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            var elementfound = wait.Until(d =>
            {
                var input = new PointerInputDevice(PointerKind.Touch);
                ActionSequence FlickUp = new ActionSequence(input);
                FlickUp.AddAction(input.CreatePointerMove(listview, 0, 0, TimeSpan.Zero));
                FlickUp.AddAction(input.CreatePointerDown(MouseButton.Left));
                FlickUp.AddAction(input.CreatePointerMove(listview, 0, -300, TimeSpan.FromMilliseconds(200)));
                FlickUp.AddAction(input.CreatePointerUp(MouseButton.Left));
                driver.PerformActions(new List<ActionSequence>() { FlickUp });

                return d.FindElement(MobileBy.AccessibilityId("This is a new Item"));
            });

            Assert.IsTrue(elementfound != null);

            driver.CloseApp();

        }
        [TestMethod]
        public void MasterDetail()
        {
            var capabilities = new AppiumOptions();
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.App, "8b831c56-bc54-4a8b-af94-a448f80118e7_sezxftbtgh66j!App");
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.PlatformName, "Windows");
            capabilities.AddAdditionalAppiumOption(MobileCapabilityType.DeviceName, "WindowsPC");

            var _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            _appiumLocalService.Start();
            var driver = new WindowsDriver(_appiumLocalService, capabilities);

            // tap on second item
            var el1 = driver.FindElement(MobileBy.AccessibilityId("Second item"));

            el1.Click();
            var el2 = driver.FindElement(MobileBy.AccessibilityId("ItemText"));
            Assert.IsTrue(el2.Text == "Second item");

            var backButton = driver.FindElement(MobileBy.AccessibilityId("Back"));
            backButton.Click();

            var el3 = driver.FindElement(MobileBy.AccessibilityId("Fourth item"));
            Assert.IsTrue(el3 != null);

            driver.CloseApp();
        }
    }
}
