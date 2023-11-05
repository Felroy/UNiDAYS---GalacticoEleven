using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json.Nodes;
using TechTalk.SpecFlow;
using UNiDAYS_API_Test.Utils;

namespace UNiDAYS_API_Test.StepDefinitions
{

    [Binding]
    public class Tests
    {   
        //Add json server URL to below method in Class1
        public static requestDetails.dataObjects requestToken = new requestDetails.dataObjects();
        public static requestDetails.locationDetails locationDetails = new requestDetails.locationDetails();

        [Given(@"I send a POST request and receive access token in the response")]
        public void TestPostAuthLogin()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var restClient = new RestClient("http://localhost:8000/");
            var request = new RestRequest("auth/login", Method.Post);
            request.AddBody(new
            {
                email = "techie@email.com",
                password = "techie"
            });

            var response = restClient.Post(request);
            var content = response.Content;
            dynamic token = JsonConvert.DeserializeObject(content);
            requestToken.accessToken = token.access_token;
            Console.WriteLine(requestToken.accessToken);
        }

        [Given(@"I send a GET request to /location endpoint and get a 200 OK response")]
        public void TestGetLocations()
        {
            var request = new RestRequest("locations", Method.Get);
            var authenticator = new JwtAuthenticator(requestToken.accessToken);

            var options = new RestClientOptions("http://localhost:8000/")
            {
                Authenticator = authenticator,
            };
            var restClient = new RestClient(options);

            var response = restClient.Get(request);
            var statusDescription = response.StatusDescription;
            //try
            Assert.That(statusDescription, Is.EqualTo("OK"));
            //catch
        }

        [Given(@"I send a GET request to /location, I should get location name of provided ID")]
        public void TestGetLocationByID()
        {
            //TestGetLocations();
            var request = new RestRequest("locations", Method.Get);
            request.AddParameter("id", "1");
            var authenticator = new JwtAuthenticator(requestToken.accessToken);

            var options = new RestClientOptions("http://localhost:8000/")
            {
                Authenticator = authenticator,
            };
            var restClient = new RestClient(options);

            var response = restClient.Get(request);
            dynamic content = response.Content;
            var statusDescription = response.StatusDescription;

            Console.WriteLine(content);

            //try
            Assert.That(statusDescription, Is.EqualTo("OK"));
            //catch
        }
    }
}

