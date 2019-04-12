drop database if exists wdda;
drop user if exists wdda@localhost;
create database wdda default character set latin1 default collate latin1_general_ci;
create user wdda@localhost identified by 'Eeph9Wae2kooWoochozouh7jo2caj5';
grant all privileges on wdda.* to wdda@localhost;
