docker-up:
	docker compose up -d

docker-down:
	docker compose down

.PHONY:
	docker-run docker-rm
