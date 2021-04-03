# DishMaster
## Running in production
1. Create `.env` based on `env.example` containing SMTP credentials and JWT key.
2. Run `docker-compose up -d`.

Note: if you don't plan on running with nginx-proxy, you should change the expose entry in `docker-compose.yml` to 80:8000 or something similar.

## Running in development
1. Make sure `dotnet-sdk-3.1` is installed on your system.
2. Create `appsettings.Development.json` based on `appsettings.Development.Example.json` with valid settings. Set the connection string to point to a running MySQL server with valid credentials, and set MySqlVersion appropriately. It is recommended to set an SMTP server, but it can also be disabled by setting `Smtp:Enabled=false`.
3. Run `dotnet watch run`.
