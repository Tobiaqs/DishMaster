FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
RUN apt-get update -y && \
    apt-get install -y wait-for-it
COPY bin/Release/netcoreapp3.1/publish/ /app/
WORKDIR /app/
CMD ["wait-for-it", "-h", "db", "-p", "3306", "--", "dotnet", "wie-doet-de-afwas.dll"]
