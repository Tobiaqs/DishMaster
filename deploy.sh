#!/bin/bash
rm -rf bin/Release
dotnet publish -c Release
cd bin/Release/netcoreapp2.1
mv publish/ wdda.env/
zip -rq deployment.zip wdda.env/
scp deployment.zip tobiass.nl:
ssh root@tobiass.nl systemctl stop wdda
ssh tobiass.nl mv ~/wdda.env/wdda.db ~/wdda.db
ssh tobiass.nl rm -rf ~/wdda.env/
ssh tobiass.nl unzip ~/deployment.zip
ssh tobiass.nl rm ~/deployment.zip
ssh tobiass.nl mv ~/wdda.db ~/wdda.env/wdda.db
cd ../../../
rm -rf bin/Release
ssh root@tobiass.nl systemctl start wdda
