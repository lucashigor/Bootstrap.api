Feature: Configuration

@Add
Scenario: Create new Configuration
	Given a valid configuration
	When the configuration are sended to request
	Then the id should not be null