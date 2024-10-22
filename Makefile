generate-allure-report:
	cp -R allure-reports/history allure-results
	rm -rf allure-reports
	allure generate allure-results -o allure-reports
	allure serve allure-results -p 10000

unit-tests:
	dotnet test --filter "FullyQualifiedName~UnitTests" --no-build --no-restore

integration-tests:
	docker compose up -d
	dotnet test --filter "FullyQualifiedName~IntegrationTests" --no-build --no-restore
	docker compose down

concat-reports:
	mkdir allure-results
	pwd
	ls
	ls UTests
	ls ITests
	cp UTests/allure-results/* allure-results/
	cp ITests/allure-results/* allure-results/

.PHONY:
	generate-allure-report unit-tests integration-tests concat-reports
