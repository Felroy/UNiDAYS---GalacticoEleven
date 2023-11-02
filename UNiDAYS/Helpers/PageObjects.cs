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
        private string name;
        private string team;
        private string competition;

        public string Name { get { return name; } set { name = value; } }
        public string Team { get { return team; } set { team = value; } }
        public string Competition { get { return competition; } set { competition = value; } }

    }
}
