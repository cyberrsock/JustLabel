Feature: Reset password with 2FA

  Scenario: User reset password with 2FA
    When User send "PUT" request to "/api/v2/users"
    Then the response on /api/v2/users code should be 200
    When User send "PUT" request to "/api/v2/users/123456" 
    Then the response on /api/v2/users/123456 code should be 200
