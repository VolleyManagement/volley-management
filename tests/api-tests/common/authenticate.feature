@ignore
Feature: Retrieve and store authentication token to test protected API parts

    Background: Set variables
        * def auth0Url = 'https://' + auth0.domain + '.eu.auth0.com'
    Scenario:
        Given url auth0Url
        And path 'oauth/token'
        And request
            """
            {
            "client_id": #(auth0.clientId),
            "client_secret": #(auth0.clientSecret),
            "audience": #(auth0.audience),
            "grant_type": "client_credentials"
            }
            """
        When method post
        Then status 200

        * def access_token = response.access_token