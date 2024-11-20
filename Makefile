docker-up:
	docker compose up -d

docker-down:
	docker compose down

static-tests:
	dotnet build --project GUI

.PHONY:
	docker-run docker-rm
