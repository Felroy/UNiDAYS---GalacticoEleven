Feature: LeagueCreation
Log onto GalacticoEleven and create a new league
 
@Scenario1
Scenario: Launch GalacticoEleven and create a new league
Given I launch GalacticoEleven
And Successfully log in using <username> and <password>
When I click on 'Create' on the 'Leagues' page
Then I enter a 'Name', 'Team' and select a 'Competition'
|      Key      |   Value     |
|      name     |  galactico  |
|      team     |  GalCo      |
And Verify successful creation of new league

Examples: 
| username                    | password |
| felroygalactico@yopmail.com | Test@123 |

@Scenario2
Scenario: Logout after League creation and trigger password recovery via email
Given I log out
And I click on 'Forgotten Password'
When I enter my email address: <username>
And I click on 'Reset Password'
Then I should receive a password recovery email for <username>

Examples: 
| username                    | password |
| felroygalactico@yopmail.com | Test@123 |