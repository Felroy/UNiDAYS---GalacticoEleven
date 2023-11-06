using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SpecflowSelenium.Utils;
using SpecflowSelenium.Helpers;
using System.Diagnostics;

namespace SpecflowSelenium.StepDefinitions
{
    
    public class GalacticoTest
    {
        //Initialize Chromedriver
        public static string path = Path.Combine(Directory.GetCurrentDirectory());
        public static ChromeDriver driver = new ChromeDriver(path);
        public static PageObjects.storeLeagueDetails leagueDetails = new PageObjects.storeLeagueDetails();


        [Binding]
        //======================Scenario 1: Logging onto GalacticoEleven and creating a new league===========================
        public class createLeagueAndVerify
        {
            public WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //Launching GalacticoEleven
            [Given(@"I launch GalacticoEleven")]
            public void launchWebpage()
            {
                driver.Navigate().GoToUrl("https://www.galacticoeleven.com/#!/home");
                Assert.IsTrue(driver.Title.ToLower().Contains("galactico"));
            }

            //Logging in
            [Given(@"Successfully log in using (.*) and (.*)")]
            public void Login(string username, string password)
            {
                //Find and click the login feature on the homepage
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
            public void ClickOnCreateLeague()
            {
                IWebElement createTab = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Create")));
                createTab.Click();
            }

            //Entering details and creating league
            [Then(@"I enter a 'Name', 'Team' and select a 'Competition'")]
            public void EnterLeagueDetails(Table table)
            {
                IWebElement leagueName = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='Name']")));
                IWebElement teamName = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@placeholder='Team name']")));

                //using ID for competition. Find dropdown -> get options -> pass to Helper function to randomize for dynamic test
                SelectElement compDropDown = new SelectElement(driver.FindElement(By.Id("competition")));
                IList<IWebElement> options = compDropDown.Options;
                string competitionName = PageObjects.dropDownOptions(options);
                compDropDown.SelectByText(competitionName);

                //Refer to Helpers folder
                var dictionary = TableExtensions.ToDictionary(table);
                leagueName.SendKeys(dictionary["name"]);
                teamName.SendKeys(dictionary["team"]);

                //Setting values to instance of ../Helpers/PageObjects to carry over across the project
                leagueDetails.Name = dictionary["name"];
                leagueDetails.Team = dictionary["team"];
                leagueDetails.Competition = competitionName;

                //Pressing the create button
                IWebElement createBtn = driver.FindElement(By.XPath("//*[@id=\"main\"]/div[1]/ui-view/form/div[4]/a[2]"));
                createBtn.Click();
            }

            [Then(@"Verify successful creation of new league")]
            public void VerifyLeagueCreation()
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"main\"]/div[1]/ui-view")));
                IWebElement element = driver.FindElement(By.XPath("//*[@id=\"main\"]/div[1]/ui-view"));

                // Get first league with specified XPath - new league always takes these EXACT same XPaths               
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"main\"]/div[1]/ui-view/div[1]")));
                IWebElement newLeague = element.FindElement(By.XPath("//*[@id=\"main\"]/div[1]/ui-view/div[1]"));

                string newLeagueName = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"main\"]/div[1]/ui-view/div[1]/h5"))).Text;
                string newLeagueTeam = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"main\"]/div[1]/ui-view/div[1]/h5/small[2]"))).Text;
                string newLeagueComp = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"main\"]/div[1]/ui-view/div[1]/h5/small[1]"))).Text;
                var test = PageObjects.parseLeagueDetails(newLeagueName, newLeagueTeam, newLeagueComp);

                try
                {
                    if (newLeague != null)
                    {
                            Console.WriteLine("Expected - Actual: " + leagueDetails.Name + " - " + test.Item1);
                            Assert.True(leagueDetails.Name.Equals(test.Item1));

                            Console.WriteLine("Expected - Actual: " + leagueDetails.Team + " - " + test.Item2);
                            Assert.True(leagueDetails.Team.Equals(test.Item2));

                            Console.WriteLine("Expected - Actual: " + leagueDetails.Competition + " - " + test.Item3);
                            Assert.True(leagueDetails.Competition.Equals(test.Item3)); 
                    }

                }
                catch (Exception ex)
                { 
                    Console.WriteLine("Element 'League' null - Check error trace: " + ex.Message);
                }                
            }
        }

        [Binding]
        //====================== Scenario: Test email service for password recovery ======================
        public class ForgottenPasswordRecovery
        {
            public WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            [Given(@"I log out")]
            public void GivenILogOut()
            {
                driver.FindElement(By.XPath("//a[@href='#!/logout']")).Click();
            }

            [Given(@"I click on 'Forgotten Password'")]
            public void GivenIClickOn()
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@href='#!/forgot']"))).Click();
            }

            [When(@"I enter my email address: (.*)")]
            public void WhenIEnterMyEmailAddress(string username)
            {
                Console.WriteLine(username);
                IWebElement emailField = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//form[@name='forgotForm']/div/input[@placeholder='Email']")));
                emailField.Click();
                emailField.SendKeys(username);
            }

            [When(@"I click on 'Reset Password'")]
            public void WhenIClickOnReset()
            {
                IWebElement resetBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"main\"]/div[1]/div/div/div/form/div[2]/button")));
                resetBtn.Click();
            }

            [Then(@"I should receive a password recovery email for (.*)")]
            public void ReceiveAPasswordRecoveryEmail(string username)
            {
                //Switch to new tab, open up yopmail
                driver.SwitchTo().NewWindow(WindowType.Tab);
                string emailDomain = PageObjects.parseEmail(username);
                driver.Navigate().GoToUrl(emailDomain);
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("accept"))).Click();
                IWebElement emailField = driver.FindElement(By.Id("login"));
                emailField.SendKeys(username);
                //Thread.Sleep to give email service some time to receive email
                Thread.Sleep(7000);
                IWebElement loginBtn = driver.FindElement(By.XPath("//*[@id=\"refreshbut\"]/button"));
                loginBtn.Click();
                //switch to inbox iframe here
                driver.SwitchTo().Frame(driver.FindElement(By.Name("ifmail")));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/header/div[3]/div[4]/span"))).Click();
                IWebElement emailTime = driver.FindElement(By.XPath("/html/body/header/div[3]/div[4]/span"));
                //Call compareTime from PageObjects to parse and compare time passed between test and NOW
                Assert.True(PageObjects.compareTime(emailTime.Text) == true);
                Console.WriteLine(PageObjects.compareTime(emailTime.Text) == true);

                driver.Quit();
            }
        }
    }
}