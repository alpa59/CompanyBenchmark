     IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DapperDatabase')
     BEGIN
         CREATE DATABASE DapperDatabase;
     END
     