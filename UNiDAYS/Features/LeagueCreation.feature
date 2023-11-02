Feature: LeagueCreation
Log onto GalacticoEleven and create a new league
 
@mytag
Scenario: Launch GalacticoEleven and create a new league
Given I launch GalacticoEleven
And Successfully log in using <username> and <password>
When I click on 'Create' on the 'Leagues' page
Then I enter a 'Name', 'Team' and select a 'Competition'
|   Key        |    Value    |
|      name     |  galactico      |
|      team     |  UNiGal      |
|    competition       |  EPL 2023/24      |
And Verify successful creation of new league

Examples: 
| username                    | password |
| felroygalactico@yopmail.com | Test@123 |