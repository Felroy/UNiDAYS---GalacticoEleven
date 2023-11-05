Feature: Run local json server and test APIs


@tag1
Scenario: 1. Send POST request and receive a bearer token
	Given I send a POST request and receive access token in the response

Scenario: 2. Send GET request to /location endpoint and get status 200
	Given I send a GET request to /location endpoint and get a 200 OK response

Scenario: 3. Send GET request to /location endpoint with location ID
	Given I send a GET request to /location, I should get location name of provided ID
