#!/bin/bash
PORT=12000
NAME=dishmaster_develop_database

docker start $NAME || docker run \
  -e MYSQL_ROOT_PASSWORD=aeph4emiequeeCuuc0xae9yai4Ve4beBusieD3fu \
  -e MYSQL_DATABASE=app \
  -v ${NAME}_volume:/var/lib/mysql \
  -p $PORT:3306 \
  -d \
  --restart unless-stopped \
  --name $NAME \
  mariadb:10.5.9
