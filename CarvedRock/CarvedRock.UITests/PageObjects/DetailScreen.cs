using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;

namespace CarvedRock.UITests.PageObjects
{
    internal class DetailScreen
    {
        private CarvedRockApplication application;

        public DetailScreen(CarvedRockApplication application)
        {
            this.application = application;
        }

        public MainScreen Dismiss()
        {
            application.Driver.PressKeyCode(AndroidKeyCode.Back);
            return new MainScreen(application);
        }

        public bool isItemText(string itemText)
        {
            var el2 = application.Driver.FindElement(MobileBy.AccessibilityId("ItemText"));
            return el2.Text == itemText;
        }
    }
}