#!/bin/bash
PORT=12000
PORT_ADMINER=11000
NAME=dishmaster_develop_database

docker network create ${NAME}_network

docker start $NAME || docker run \
  -e MYSQL_ROOT_PASSWORD=aeph4emiequeeCuuc0xae9yai4Ve4beBusieD3fu \
  -e MYSQL_DATABASE=app \
  -v ${NAME}_volume:/var/lib/mysql \
  -p $PORT:3306 \
  --net ${NAME}_network \
  -d \
  --restart unless-stopped \
  --name $NAME \
  mariadb:10.5.9
docker start ${NAME}_adminer || docker run \
  -e ADMINER_DEFAULT_SERVER=$NAME \
  -p $PORT_ADMINER:8080 \
  --net ${NAME}_network \
  -d \
  --restart unless-stopped \
  --name ${NAME}_adminer \
  adminer:4.8.0
