USE master;
GO

IF  EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'VolleyManagement')
DROP DATABASE VolleyManagement;
GO

CREATE DATABASE VolleyManagement
COLLATE Cyrillic_General_CI_AS;
GO

USE VolleyManagement;
GO

/*************************************************
  Creating tables
*************************************************/


CREATE TABLE dbo.Tournament(
  Id int identity(1, 1) NOT NULL
    CONSTRAINT PK_Tournament_Id PRIMARY KEY CLUSTERED,
  Name nvarchar(80) NOT NULL,
  Season nvarchar(40) NOT NULL
);
GO

/*************************************************
  Inserting data
*************************************************/

INSERT INTO dbo.Tournament(
  [Name],
  [Season]
)VALUES
  ('Первый чемпионат любительской лиги', '2012/2013'),
  ('Второй чемпионат любительской лиги', '2013/2014'),
  ('Третий чемпионат любительской лиги', '2014/2015'),
  ('Четвертый чемпионат любительской лиги', '2014/2015'),
  ('Пятый чемпионат любительской лиги', '2014/2015');
GO
