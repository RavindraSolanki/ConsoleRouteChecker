# .NET ConsoleRouteChecker
This is a console application for checking the status of a given road

### Prerequisites
1. [Specflow for Visual Studio 2017](https://specflow.org/getting-started/) is required for test projects
## Getting started
1. Clone this respository
2. Open the solution into VisualStudio
3. Replace in the app.config of the ConsoleApp the TFL-API credentials
  * `<add key="TflApiApplicationId" value="add-tfl-api-application-id"/>`
  * `<add key="TflApiApplicationKeys" value="add-tfl-api-application-keys"/>`
4. Build the application
5. Run in a command prompt or PowerShell `RouteChecker.ConsoleApp.exe <road-id>` 
  * e.g.  `RouteChecker.ConsoleApp.exe a2`