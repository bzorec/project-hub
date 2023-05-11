#!/bin/bash
echo "Shutting down api container"
docker compose -f docker-compose-api.yml down
echo "Building api image"
docker compose -f docker-compose-api.yml build
echo "Starting api container"
docker compose -f docker-compose-api.yml up -d