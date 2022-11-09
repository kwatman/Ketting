# Ketting
[![build and test](https://github.com/kwatman/Ketting/actions/workflows/test.yml/badge.svg)](https://github.com/kwatman/Ketting/actions/workflows/test.yml)

Ketting is a blockchain made for a school project

# how to run on your pc

first cd into the project and run:
docker build . -t ketkoinserver

then cd in the front end folder and run:
docker build . -t ketkoin-frontend

go back up one folder and run the command:
docker network create -d bridge ketkoin-network 

then go in the docker compose file and change every ip adress with your ip adress. keep the port number at the end. after that you can run:
docker compose up

front end is then available on
{ip}:3000/html/index.html

the ketkoin servers run on port 2400 through 2404
