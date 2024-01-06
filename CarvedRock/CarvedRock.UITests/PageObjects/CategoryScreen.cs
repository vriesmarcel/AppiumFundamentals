using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;
using System;

namespace CarvedRock.UITests.PageObjects
{
    internal class CategoryScreen
    {
        private CarvedRockApplication application;

        public CategoryScreen(CarvedRockApplication application)
        {
            this.application = application;
        }

        internal void CreateNewCategory(string categoryName)
        {

            CreateNewCategory(application.Driver, categoryName);
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

    }
}