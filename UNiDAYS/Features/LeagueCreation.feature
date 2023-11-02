Feature: LeagueCreation
Log onto GalacticoEleven and create a new league
 
@mytag
Scenario: Launch GalacticoEleven and create a new league
Given I launch GalacticoEleven
And Successfully log in using registered credentials
|      Key 		  |	     Value						|
|   username	  |		felroygalactico@yopmail.com    |
|   password	  |		Test@123    |
When I click on 'Create' on the 'Leagues' page
Then I enter a 'Name', 'Team' and select a 'Competition'
And Create a new league
Then I verify successful creation of new league



