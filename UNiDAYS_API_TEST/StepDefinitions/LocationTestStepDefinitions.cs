using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using TechTalk.SpecFlow;
using UNiDAYS_API_Test.Utils;

namespace UNiDAYS_API_Test.StepDefinitions
{

    [Binding]
    public class Tests
    {
        string url = "http://localhost:8000/";

        //Get utility methods from Utility.cs
        public static requestDetails.dataObjects requestToken = new requestDetails.dataObjects();
        public static requestDetails.locationDetails locationDetails = new requestDetails.locationDetails();

        //Code comments in Utility.cs
        //Scenario: 1. Send POST request and receive a bearer token
        [Given(@"POST request to 'auth/login' to receive bearer token")]
        public void TestPostAuthLogin()
        {
            var restClient = new RestClient(url);
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
        }

        //Scenario: 2. Send GET request to '/locations' and get status 200
        [Given(@"GET request to '/locations' endpoint and get a 200 OK response")]
        public void TestGetLocations()
        {
            var request = new RestRequest("locations", Method.Get);
            var authenticator = new JwtAuthenticator(requestToken.accessToken);

            var options = new RestClientOptions(url)
            {
                Authenticator = authenticator,
            };
            var restClient = new RestClient(options);

            var response = restClient.Get(request);
            var statusDescription = response.StatusDescription;

            Assert.That(statusDescription, Is.EqualTo("OK"));
        }
        //Scenario: 3. Send GET request to '/locations' with location ID
        [Given(@"GET request to '/locations' with parameter 'id' = 1")]
        public void GetLocationByID()
        {
            var request = new RestRequest("locations", Method.Get);
            request.AddParameter("id", "1");
            var authenticator = new JwtAuthenticator(requestToken.accessToken);

            var options = new RestClientOptions(url)
            {
                Authenticator = authenticator,
            };
            var restClient = new RestClient(options);

            var response = restClient.Get(request);
            dynamic content = response.Content;
            var obj = JsonConvert.DeserializeObject<dynamic>(content);
            string locationId = obj[0].id;
            string locationName = obj[0].name;

            var statusDescription = response.StatusDescription;
            Assert.That(locationId
                , Is.EqualTo("1"));
            Assert.That(locationName, Is.EqualTo("Location001"));
            Assert.That(statusDescription, Is.EqualTo("OK"));
        }
        //Scenario: 4. Send POST request to '/locations' with location ID
        [Given(@"POST request to /locations with request body { 'name' : 'Location004'}")]
        public void PostLocationName()
        {
            var request = new RestRequest("locations", Method.Post);
            var authenticator = new JwtAuthenticator(requestToken.accessToken);

            var options = new RestClientOptions(url)
            {
                Authenticator = authenticator,
            };
            var restClient = new RestClient(options);
            request.AddBody(new
            {
                name = "Location004"
            });

            restClient.Post(request);
        }
        [Then(@"GET request to 'locations/4' endpoint")]
        public void GetPostedLocationName()
        {
            requestDetails.requestFunctions.GetPostedLocationNameHandler("Location004", url);
        }

        //Scenario: 5. Send PUT request to /locations/4 endpoint
        [Given(@"PUT request to '/locations/4' with request body { 'name' : 'Location005'}")]
        public void PutLocationName()
        {
            var request = new RestRequest("locations/4", Method.Put);
            var authenticator = new JwtAuthenticator(requestToken.accessToken);

            var options = new RestClientOptions(url)
            {
                Authenticator = authenticator,
            };
            var restClient = new RestClient(options);
            request.AddBody(new
            {
                name = "Location005"
            });
            restClient.Put(request);
        }

        [Then(@"GET request to 'locations/4' endpoint returns { 'name' : 'Location005'}")]
        public void GetPutLocationName()
        {
            requestDetails.requestFunctions.GetPostedLocationNameHandler("Location005", url);
        }
        //Scenario: 6. Send DELETE request to '/locations/4'
        [Given(@"DELETE request to '/locations/4'")]
        public void DeleteLocation()
        {
            var request = new RestRequest("locations/4", Method.Delete);
            var authenticator = new JwtAuthenticator(requestToken.accessToken);

            var options = new RestClientOptions(url)
            {
                Authenticator = authenticator,
            };
            var restClient = new RestClient(options);
            var response = restClient.Delete(request);
            var statusDescription = response.StatusDescription;

            Assert.That(statusDescription, Is.EqualTo("OK"));
        }
        //Scenario: 7. BONUS_Send POST request to '/locations' to restore deleted location
        [Given(@"POST request to /locations with request body {'id':4, 'name' : 'Location004'}")]
        public void RestoreDeletedLocation()
        {
            var request = new RestRequest("locations", Method.Post);
            var authenticator = new JwtAuthenticator(requestToken.accessToken);

            var options = new RestClientOptions(url)
            {
                Authenticator = authenticator,
            };
            var restClient = new RestClient(options);
            request.AddBody(new
            {
                id = 4,
                name = "Location004"
            });

            restClient.Post(request);
        }
        [Then(@"GET request to verify previously deleted location has been restored")]
        public void GetRestoredLocation()
        {
            requestDetails.requestFunctions.GetPostedLocationNameHandler("Location004", url);
        }
    }
}

