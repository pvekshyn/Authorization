Feature: Permission

Scenario: No access to create permission
	Given I am logged in as user without permissions
	When permission created
	Then forbidden result

Scenario: Create new permission
	Given I am logged in as admin
	When permission created
	Then success result
	And permission in authorization service

Scenario: No access to delete permission
	Given I am logged in as user without permissions
	When permission deleted
	Then forbidden result

Scenario: Delete permission
	Given I am logged in as admin
	And permission created
	When permission deleted
	Then success result
	And permission not in authorization service