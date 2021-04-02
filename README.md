# Wie doet de afwas?
## Running in production
1. Create `appsettings.Production.json` based on `appsettings.Example.json` with valid settings. Set the connection string to match `MySql__example_docker_compose`, and set `MySqlVersion` to MariaDB's version from `docker-compose.yml`.
2. Run `docker-compose up -d`.

Note: if you don't plan on running with nginx-proxy, you should change the expose entry in `docker-compose.yml` to 80:8000 or something similar.

## Running in development
1. Make sure `dotnet-sdk-3.1` is installed on your system.
2. Create `appsettings.Development.json` based on `appsettings.Example.json` with valid settings. Set the connection string to point to a running MySQL server with valid credentials, and set MySqlVersion appropriately.
2. Run `ASPNETCORE_ENVIRONMENT=Development dotnet watch run`.
