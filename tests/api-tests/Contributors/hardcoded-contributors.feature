Feature: Hardcoded Contributors API

    Fetch all contributors to the Volley Management project

    Scenario: Get All contributors

        Given url vmAppUrl
        Given path 'api/contributors'
        When method GET
        Then status 200
        And assert response.length == 2
        And match each response == {fullName:'#string', team:'#string', courseDirection:'#string'}