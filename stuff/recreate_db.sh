#!/bin/bash
mysql -u root -pAepahng0aw0aeZeeyoothohLae0quo < stuff/recreate_db.sql
dotnet ef database update
echo -e "\n😊 Database recreation finished"
echo "👋 Bye!"
