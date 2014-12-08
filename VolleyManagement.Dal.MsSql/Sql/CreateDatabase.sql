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
  Scheme tinyint NOT NULL,
  Season nvarchar(40) NOT NULL,
  [Description] nvarchar(1024) NULL,
  RegulationsLink nvarchar(1024) NULL
);
GO
