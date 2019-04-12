#!/bin/bash
mysql -u root -pAepahng0aw0aeZeeyoothohLae0quo < stuff/recreate-db.sql
ssh tobiass.nl mysqldump -u wdda -pieVeteejot6co0reil6ahLei0ohPaa wdda > wdda_pull.sql
mysql -u wdda -pEeph9Wae2kooWoochozouh7jo2caj5 -D wdda < wdda_pull.sql
rm wdda_pull.sql
echo -e "\n😊 Database pull finished"
echo "👋 Bye!"
