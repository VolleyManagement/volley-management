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

CREATE TABLE dbo.Tournaments(
  Id int identity(1, 1) NOT NULL
    CONSTRAINT PK_Tournament_Id PRIMARY KEY CLUSTERED,
  Name nvarchar(60) NOT NULL,
  Scheme tinyint NOT NULL DEFAULT 1,
  Season tinyint NOT NULL,
  [Description] nvarchar(300) NULL,
  RegulationsLink nvarchar(255) NULL
);
GO

CREATE TABLE dbo.Users(
  Id int identity(1, 1) NOT NULL 
    CONSTRAINT PK_Users_Id PRIMARY KEY CLUSTERED,
  UserName nvarchar(20) NOT NULL
    CONSTRAINT UN_Users_Login UNIQUE,
  Password char(68) NOT NULL,
  FullName nvarchar(60) NULL,
  CellPhone nvarchar(20) NUll,
  Email nvarchar(80) NOT NULL
    CONSTRAINT UN_Users_Email UNIQUE
);
GO