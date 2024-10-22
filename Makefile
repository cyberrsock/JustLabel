generate-allure-report:
	cp -R allure-reports/history allure-results
	rm -rf allure-reports
	allure generate allure-results -o allure-reports
	allure serve allure-results -p 10000

unit-tests:
	dotnet test --filter "FullyQualifiedName~UnitTests"

integration-tests:
	docker compose up -d
	dotnet test --filter "FullyQualifiedName~IntegrationTests"
	docker compose down

e2e-tests:
	docker compose up -d
	dotnet test --filter "FullyQualifiedName~E2ETests"
	docker compose down

concat-reports:
	mkdir allure-results
	cp UTests/allure-results/* allure-results/
	cp ITests/allure-results/* allure-results/

.PHONY:
	generate-allure-report unit-tests integration-tests e2e-tests concat-reports
