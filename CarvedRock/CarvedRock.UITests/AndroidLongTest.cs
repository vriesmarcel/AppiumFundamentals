using CarvedRock.UITests.PageObjects;
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
            var application = new CarvedRockApplication();
            var categoryName = "New category from automation";
            var itemName = "New Item in List";
            var description = "This is the description of the new item";

            Assert.IsTrue(application.StartApplication().
                CreateNewCategory(categoryName).
                CreateNewItemInList(itemName, description, categoryName).
                IsItemOnScreen(itemName, true)); 

            application.CloseApplication();
        }


        [TestMethod]
        public void AddNewItem()
        {
            var application = new CarvedRockApplication();
            var itemName = "New Item in List";
            var description = "This is the description of the new item";
            
            Assert.IsTrue(application.StartApplication().
                CreateNewItemInList(itemName, description).
                IsItemOnScreen(itemName, true));

            application.CloseApplication();
        }

        [TestMethod]
        public void MasterDetail()
        {
            var application = new CarvedRockApplication();
            Assert.IsTrue(application.StartApplication().
                SelectItem("Second item").
                Dismiss().
                IsItemOnScreen("Fourth item",false));
            

            application.CloseApplication();
        }


     
     
     
    
       
    }
}
