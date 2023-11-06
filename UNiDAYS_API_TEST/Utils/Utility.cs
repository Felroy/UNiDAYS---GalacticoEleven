using Newtonsoft.Json;
using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace UNiDAYS_API_Test.Utils
{
    public class requestDetails
    {   
        public static string bearerToken;
        public class dataObjects
        {
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

        public class requestFunctions
        {
            public static void GetPostedLocationNameHandler(string locName, string serverUrl)
            {
                //Prepare GET call to endpoint
                var request = new RestRequest("locations/4", Method.Get);
                var authenticator = new JwtAuthenticator(bearerToken);

                //Add authentication
                var options = new RestClientOptions(serverUrl)
                {
                    Authenticator = authenticator,
                };
                var restClient = new RestClient(options);

                //Send request and get store response
                var response = restClient.Get(request);
                dynamic content = response.Content;
                var obj = JsonConvert.DeserializeObject<dynamic>(content);
                //Store response id and name locally
                string locationId = obj.id;
                string locationName = obj.name;

                var statusDescription = response.StatusDescription;
                //Assert if id and name match, with a 200 OK response
                Assert.That(locationId
                    , Is.EqualTo("4"));
                Assert.That(locationName, Is.EqualTo(locName));
                Assert.That(statusDescription, Is.EqualTo("OK"));
            }
        }


    }
   

    
}
