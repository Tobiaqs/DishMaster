FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
RUN curl -fsSL https://deb.nodesource.com/setup_15.x | bash - && \
    apt-get install -y nodejs

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
RUN apt-get update -y && \
    apt-get install -y wait-for-it
COPY --from=build-env /app/out /app
WORKDIR /app
CMD ["wait-for-it", "-h", "database", "-p", "3306", "--", "dotnet", "DishMaster.dll"]
