Feature: A user can request from the console application the status of a road

@mytag
Scenario: A road has a displayName
	Given a valid road ID is specified
	When the client is run
	Then the road ‘displayName’ should be displayed

Scenario: A road has a statusSeverity
	Given a valid road ID is specified
	When the client is run
	Then the road ‘statusSeverity’ should be displayed as ‘Road Status’

Scenario: A roads has a statusSeverityDescription
	Given a valid road ID is specified
	When the client is run
	Then the road ‘statusSeverityDescription’ should be displayed as ‘Road Status Description’

Scenario: If a road does not exist returns an informative error 
	Given an invalid road ID is specified
	When the client is run
	Then the application should return an informative error

Scenario: If a road does not exist the application exit with an error code
	Given an invalid road ID is specified
	When the client is run
	Then the application should exit with a non-zero System Error code