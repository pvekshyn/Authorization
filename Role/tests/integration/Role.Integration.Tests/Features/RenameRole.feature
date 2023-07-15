Feature: Rename Role

Scenario: Rename role validation
	Given Rename payload with not existing role
	When Role renamed
	Then Validation error

Scenario: Rename role success
	Given Permission exist
	And Role with this permission exist
	And Rename payload valid 
	When Role renamed
	Then Success result
	And Role name changed in database
