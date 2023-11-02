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
using TechTalk.SpecFlow.CommonModels;
using SpecflowSelenium;
using SpecflowSelenium.Helpers;
using System.Diagnostics;

namespace SpecflowSelenium.StepDefinitions
{
    [Binding]
    public class LeagueCreationSteps : IDisposable
    {
        PageObjects leagueDetails = new PageObjects();
        //Initialize Chrome driver
        private ChromeDriver driver;
        public LeagueCreationSteps() => driver = new ChromeDriver(@"X:\Visual Studio\UNiDAYS\UNiDAYS\Drivers\chromedriver.exe");

        //======================Scenario 1: Logging onto GalacticoEleven and creating a new league===========================
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

        //Navigating to the League creation page
        [When(@"I click on 'Create' on the 'Leagues' page")]
        public void createLeague()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement createTab = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Create")));
            createTab.Click();

            IWebElement leagueName = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='Name']")));
            IWebElement teamName = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='Team name']")));
            IWebElement competitionName = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("competition")));
        }

        //Entering details and creating league
        [Then(@"I enter a 'Name', 'Team' and select a 'Competition'")]
        public void enterLeagueDetails(Table table)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement leagueName = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='Name']")));
            IWebElement teamName = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='Team name']")));
            //using ID for competition since the placeholder text seems to be based on year & is likely to change
            IWebElement competitionName = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("competition")));

            //Refer to Helpers folder
            var dictionary = TableExtensions.ToDictionary(table);
            leagueName.SendKeys(dictionary["name"]);
            teamName.SendKeys(dictionary["team"]);
            competitionName.SendKeys(dictionary["competition"]);
            
            //Setting values to instance of ../Helpers/PageObjects to carry over across the project
            leagueDetails.Name = dictionary["name"];
            leagueDetails.Team = dictionary["team"];
            leagueDetails.Competition = dictionary["competition"];

            //Pressing the create button
            IWebElement createBtn = driver.FindElement(By.XPath("//*[@id=\"main\"]/div[1]/ui-view/form/div[4]/a[2]"));
            createBtn.Click();

            Thread.Sleep(10000);
        }

        [Then(@"Verify successful creation of new league")]
        public void verifyLeagueCreation()
        {
            //DELETE - Testing GET/SET
            Console.WriteLine(leagueDetails.Competition.ToString());
        }
        public void Dispose()
        {
            driver.Dispose();
        }
    }
}