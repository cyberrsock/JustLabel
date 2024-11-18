docker-up:
	docker compose up -d

docker-down:
	docker compose down
	docker image rm backend postgresql-testdb

.PHONY:
	docker-run docker-rm
