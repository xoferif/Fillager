#!/bin/bash
docker-compose -f ./docker-compose.ci.build.yml up

docker build -t xoferif/fillager:latest ./Fillager/.
docker build -t fillager ./Fillager/.
#docker push xoferif/miniobackupmanager