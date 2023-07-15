Feature: Create Permission

Scenario: Create permission validation
	Given Permission name empty
	When Permission created
	Then Validation error
	And Permission not in database

Scenario: Create permission success
	Given Permission valid
	When Permission created
	Then Success result
	And Permission in database

Scenario: Concurrent create same permission twice
	Given Two permissions with same id and name
	When Permissions created
	Then One success result
	And One idempotent result

Scenario: Concurrent create permission with same name twice
	Given Two permissions with same name
	When Permissions created
	Then One success result
	And One validation error
