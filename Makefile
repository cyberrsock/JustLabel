docker-up:
	docker compose up -d

docker-down:
	docker compose down

static-tests:
	cd GUI
	dotnet build

.PHONY:
	docker-run docker-rm
