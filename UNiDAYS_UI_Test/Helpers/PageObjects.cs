using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SpecflowSelenium.Helpers
{
    public class PageObjects
    {
        public static string dropDownOptions(IList<IWebElement> dd)
        {
            Random rnd = new Random();
            int r = rnd.Next(dd.Count);
            var choice = dd[r];
            string dropDownChoice = choice.Text;
            Console.WriteLine(dropDownChoice);
            return dropDownChoice;
        }
        public class storeLeagueDetails
        {
            private string name;
            private string team;
            private string competition;

            public string Name { get { return name; } set { name = value; } }
            public string Team { get { return team; } set { team = value; } }
            public string Competition { get { return competition; } set { competition = value; } }
        }

        public static Tuple<string, string, string> parseLeagueDetails(string n, string t, string c)
        {           
            string[] nSplit = n.Trim().Split(new string[] { "\r\n", "\r", "\n", " "}, StringSplitOptions.None);
            Console.WriteLine(nSplit[0]);
            n = nSplit.First();
            t = t.Replace("Team: ", "").Trim();
            c = c.Replace("Competition: ", "").Trim();

            return new Tuple<string, string, string>(n, t, c);
        }

        public static string parseEmail(string n)
        {
            string[] nEmail = n.Split("@");
            string emailDomain = "https://" + nEmail[1];
            Assert.True(emailDomain.Equals("https://yopmail.com"));

            return emailDomain.ToString();
        }

        public static bool compareTime(string n)
        {
            bool isTestValid = false;
            n = n.Replace(",", "");
            DateTime parsedDateTime = DateTime.ParseExact(n, "dddd MMMM dd yyyy h:m:s tt", System.Globalization.CultureInfo.InvariantCulture);
            DateTime currentDateTime = DateTime.Now;
            
            string[] nTime = n.Trim().Split(new string[] { "\r\n", "\r", "\n", " " }, StringSplitOptions.None);
            if ((currentDateTime - parsedDateTime).TotalMinutes < 2)
            {
                isTestValid = true;
            }
            else
            {
                isTestValid = false;
            }
            return isTestValid;
            //Console.WriteLine(time);
            //return parsedDateTime;
        }
    }

    
}
