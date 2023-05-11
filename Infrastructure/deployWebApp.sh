#!/bin/bash
echo "Shutting down web app container"
docker compose -f docker-compose-app.yml down
echo "Building web app image"
docker compose -f docker-compose-app.yml build
echo "Starting web app container"
docker compose -f docker-compose-app.yml up -d