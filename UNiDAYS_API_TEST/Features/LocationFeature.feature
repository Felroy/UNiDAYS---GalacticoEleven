Feature: Run local json server and test APIs


@tag1
Scenario: 1. Send POST request and receive a bearer token
	Given POST request to 'auth/login' to receive bearer token

Scenario: 2. Send GET request to '/locations' and get status 200
	Given GET request to '/locations' endpoint and get a 200 OK response

Scenario: 3. Send GET request to '/locations' with location ID
	Given GET request to '/locations' with parameter 'id' = 1

Scenario: 4. Send POST request to '/locations' with location ID
	Given POST request to /locations with request body { 'name' : 'Location004'}
	Then GET request to 'locations/4' endpoint

Scenario: 5. Send PUT request to '/locations/4'
	Given PUT request to '/locations/4' with request body { 'name' : 'Location005'}
	Then GET request to 'locations/4' endpoint returns { 'name' : 'Location005'}

Scenario: 6. Send DELETE request to '/locations/4'
	Given DELETE request to '/locations/4'

Scenario: 7. BONUS_Send POST request to '/locations' to restore deleted location
	Given POST request to /locations with request body {'id':4, 'name' : 'Location004'}
	Then GET request to verify previously deleted location has been restored