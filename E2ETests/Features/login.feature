Feature: Login with 2FA

  Scenario: User login with 2FA
    When User send "PATCH" request to "/api/v2/auth"
    Then the response on /api/v2/auth code should be 200
    When User send "PATCH" request to "/api/v2/auth/123456" 
    Then the response on /api/v2/auth/123456 code should be 200
