# order-service

## Run the application

### Locally

with docker-compose: `docker compose up`

with kubernetes:

1. `helm install order-mongodb oci://registry-1.docker.io/bitnamicharts/mongodb`

2. Zoek in kubernetes secrets naar de credentials van de database en zet die in de connectionstring in deployment.yaml.

3. `helm install order-service ./helm`

## status

This dotnet 7.0 api can handle basic CRUD operations on a mongoDB database that can be run with docker-compose
