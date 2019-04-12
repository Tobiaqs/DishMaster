#!/bin/bash
cd ..
rm -rf bin/Release
dotnet publish -c Release
cd bin/Release/netcoreapp2.1
mv publish/ wdda.env/
zip -rq deployment.zip wdda.env/
scp deployment.zip tobiass.nl:
ssh root@tobiass.nl systemctl stop wdda
ssh tobiass.nl rm -rf ~/wdda.env/
ssh tobiass.nl unzip -qq ~/deployment.zip
ssh tobiass.nl rm ~/deployment.zip
cd ../../../
rm -rf bin/Release
ssh root@tobiass.nl systemctl start wdda
echo -e "\n😊 Deploy script finished"
echo "👋 Bye!"
cd stuff