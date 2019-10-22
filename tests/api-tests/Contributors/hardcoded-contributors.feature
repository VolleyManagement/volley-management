Feature: Hardcoded Contributors API

    Background: Fetch all contributors to the Volley Management project
        Given url vmAppUrl

    Scenario: Get All contributors

        Given path 'api/contributors'
        When method GET
        Then status 200
        And assert response.length == 2
        And match each response == {fullName:'#string', team:'#string', courseDirection:'#string'}

    Scenario: Get All protected contributors

        Given path 'api/contributors'
        And header Authorization = auth_header
        When method GET
        Then status 200
        And assert response.length == 2
        And match each response == {fullName:'#string', team:'#string', courseDirection:'#string'}