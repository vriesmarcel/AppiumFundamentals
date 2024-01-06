using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Support.UI;

namespace CarvedRock.UITests.PageObjects
{
    internal class NewItemScreen
    {
        private CarvedRockApplication application;

        public NewItemScreen(CarvedRockApplication application)
        {
            this.application = application;
        }

        internal void CreateNewItemInList(string itemName, string description, string categoryName = null)
        {
            CreateNewItemInList(application.Driver,itemName,description, categoryName);
            WaitForProgressBar(application.Driver);
        }
        private void CreateNewItemInList(AndroidDriver driver,string itemName,string description, string category = null)
        {
            var el1 = driver.FindElement(MobileBy.AccessibilityId("Add"));
            el1.Click();

            //create new item 
            var elItemText = driver.FindElement(MobileBy.AccessibilityId("ItemText"));
            elItemText.Clear();
            elItemText.SendKeys(itemName);

            var elItemDetail = driver.FindElement(MobileBy.AccessibilityId("ItemDescription"));
            elItemDetail.Clear();
            elItemDetail.SendKeys(description);
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

    }
}