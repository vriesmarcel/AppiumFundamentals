using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace CarvedRock.UITests.PageObjects
{
    internal class MainScreen
    {
        private CarvedRockApplication application;

        public MainScreen(CarvedRockApplication application)
        {
            this.application = application;
        }

        internal bool IsItemOnScreen(string itemName, bool flickInview)
        {
            AppiumElement elementfound = WaitForItemInview(application.Driver, itemName, flickInview);
            return elementfound != null;
        }
        private AppiumElement WaitForItemInview(AndroidDriver driver, string itemName, bool flickInView)
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
                    if (textView.Text == itemName)
                        return textView;
                }
                return null;
            };

            AppiumElement elementfound = null;

            if (flickInView)
            {
                var wait = new DefaultWait<AndroidDriver>(driver)
                {
                    Timeout = TimeSpan.FromSeconds(60),
                    PollingInterval = TimeSpan.FromMilliseconds(1000)
                };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

                elementfound = wait.Until(d =>
                {
                    FlickUp(driver, listview);
                    return FindElementAction();
                });
            }
            else
            {
                elementfound = FindElementAction();
            }
            return elementfound;
        }

        private void FlickUp(AndroidDriver driver, AppiumElement listview)
        {
            var input = new PointerInputDevice(PointerKind.Touch);
            ActionSequence FlickUp = new ActionSequence(input);
            FlickUp.AddAction(input.CreatePointerMove(listview, 0, 0, TimeSpan.Zero));
            FlickUp.AddAction(input.CreatePointerDown(MouseButton.Left));

            FlickUp.AddAction(input.CreatePointerMove(listview, 0, -600, TimeSpan.FromMilliseconds(200)));
            FlickUp.AddAction(input.CreatePointerUp(MouseButton.Left));
            driver.PerformActions(new List<ActionSequence>() { FlickUp });
        }

        public DetailScreen SelectItem(string itemName)
        {
            // tap on second item
            var el1 = application.Driver.FindElement(MobileBy.AccessibilityId(itemName));
            el1.Click();
            return new DetailScreen(application);
        }

        internal MainScreen CreateNewCategory(string categoryName)
        {
            var categoryScreen = new CategoryScreen(application);
            categoryScreen.CreateNewCategory(categoryName);
            return this;
        }

        internal MainScreen CreateNewItemInList(string itemName, string description, string categoryName=null)
        {
            var newItemScreen = new NewItemScreen(application);
            newItemScreen.CreateNewItemInList(itemName, description, categoryName);
            return this;
        }
    }
}