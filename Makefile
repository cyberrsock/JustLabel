docker-up:
	docker compose up -d

docker-down:
	docker compose down
	docker image rm testing-auth-app alpine:latest

.PHONY:
	docker-run docker-rm
