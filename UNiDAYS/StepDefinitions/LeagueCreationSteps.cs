using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using SpecflowSelenium.Utils;

namespace SpecflowSelenium.StepDefinitions
{
    [Binding]
    public class LeagueCreationSteps : IDisposable
    {
        //Initialize Chrome driver
        private ChromeDriver driver;
        public LeagueCreationSteps() => driver = new ChromeDriver(@"X:\Visual Studio\UNiDAYS\UNiDAYS\Drivers\chromedriver.exe");

        //Scenario 1: Logging onto GalacticoEleven and creating a new league
        //======================

        //Launching GalacticoEleven
        [Given(@"I launch GalacticoEleven")]
        public void launchWebpage()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl("https://www.galacticoeleven.com/#!/home");
            Assert.IsTrue(driver.Title.ToLower().Contains("galactico"));
        }

        //Logging in
        [Given(@"Successfully log in using (.*) and (.*)")]
        public void Login(string username, string password)
        {
            //Find and click the login feature on the homepage
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement login = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"sidebar\"]/div/ul[3]/li/a")));
            if (login != null)
            {
                //Find and input username and password onto the login form, then click on 'Login' - Located using XPath(.//placeholder) to change things up
                Assert.IsTrue(login.Text.ToLower().Contains("login"));
                login.Click();
                IWebElement emailField = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='Email']")));
                IWebElement pwdField = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='Password']")));

                //Input username and password from Feature file, then click on 'Login'
                emailField.SendKeys(username);
                pwdField.SendKeys(password);
                IWebElement loginBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"main\"]/div[1]/div/div/div/form/div[3]/button")));
                loginBtn.Click();
            }
        }

        public void Dispose()
        {
            driver.Dispose();
        }
    }
}