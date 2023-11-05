using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNiDAYS_API_Test.Utils
{
    public class requestDetails
    {
        public class dataObjects
        {
            private string bearerToken;
            public string accessToken   // property
            {
                get { return bearerToken; }   // get method
                set { bearerToken = value; }  // set method
            }
        }
        public class locationDetails
        {
            public int id { get; set; }
            public string locationName { get; set; }
        }

    }
    
}
