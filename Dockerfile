FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY bin/Release/netcoreapp3.1/ /app/
WORKDIR /app/
ENTRYPOINT ["dotnet", "wie-doet-de-afwas.dll"]
