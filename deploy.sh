#!/bin/bash
rm -rf bin/Release
dotnet publish -c Release
cd bin/Release/netcoreapp2.1
zip -r ../../../build.zip publish/
cd ../../../
scp build.zip tobiass.nl:
ssh tobiass.nl ~/stop_wdda.sh
ssh tobiass.nl mv ~/wdda.env/wdda.db ~/wdda.db
ssh tobiass.nl rm -rf ~/wdda.env/
ssh tobiass.nl unzip build.zip
ssh tobiass.nl mv ~/publish/ ~/wdda.env/
ssh tobiass.nl mv ~/wdda.db ~/wdda.env/wdda.db
rm build.zip
ssh tobiass.nl rm ~/build.zip
ssh tobiass.nl ~/start_wdda.sh