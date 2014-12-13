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
  Name nvarchar(60) NOT NULL
    CONSTRAINT UN_Tournament_Name UNIQUE,
  Scheme tinyint NOT NULL DEFAULT 1
	CONSTRAINT CK_Tournament_Scheme CHECK 
      (Scheme>=1 and Scheme<=3),
  Season nvarchar(9) NOT NULL,
  [Description] nvarchar(300) NULL,
  RegulationsLink nvarchar(255) NULL
);
GO
