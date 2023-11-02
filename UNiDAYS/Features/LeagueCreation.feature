Feature: LeagueCreation
Log onto GalacticoEleven and create a new league
 
@mytag
Scenario: Launch GalacticoEleven and create a new league
Given I launch GalacticoEleven
And Successfully log in using <username> and <password>

Examples: 
|      username	  |	     password						|
|   felroygalactico@yopmail.com	  |		Test@123    |