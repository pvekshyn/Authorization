Feature: Permission

Scenario: Create new permission
	Given I am logged in as admin
	When permission created
	Then permission in authorization service

Scenario: Delete permission
	Given I am logged in as admin
	And permission created
	When permission deleted
	Then permission not in authorization service