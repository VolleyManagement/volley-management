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
  RegulationsLink nvarchar(255) NULL,
  ApplyingPeriodStart date NOT NULL,
  ApplyingPeriodEnd date NOT NULL,
  GamesStart date NOT NULL,
  GamesEnd date NOT NULL,
  TransferStart date NOT NULL,
  TransferEnd date NOT NULL
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

CREATE TABLE dbo.Players(
  Id int identity(1, 1) NOT NULL 
    CONSTRAINT PK_Players_Id PRIMARY KEY CLUSTERED,
  FirstName nvarchar(60) NOT NULL,
  LastName nvarchar(60) NOT NULL,
  TeamId int NULL,
  BirthYear int NULL,
  Height int NULL,
  Weight int NULL
);
GO

<<<<<<< HEAD
CREATE TABLE dbo.ContributorTeam(
  Id int identity(1, 1) NOT NULL 
    CONSTRAINT PK_ContributorTeam_Id PRIMARY KEY CLUSTERED,
  Name nvarchar(20) NOT NULL,
  CourseDirection nvarchar(20) NOT NULL
);
GO

CREATE TABLE dbo.Contributors(
  Id int identity(1, 1) NOT NULL 
    CONSTRAINT PK_Contributors_Id PRIMARY KEY CLUSTERED,
  Name nvarchar(30) NOT NULL,
  ContributorTeamId int FOREIGN KEY  REFERENCES ContributorTeam(Id)
);
GO

=======
CREATE TABLE dbo.Teams(
  Id int identity(1, 1) NOT NULL
    CONSTRAINT PK_Teams_Id PRIMARY KEY CLUSTERED,
  Name nvarchar(30) NOT NULL,     
  CaptainId int NOT NULL
    CONSTRAINT FK_Teams_CaptainId_Players_Id FOREIGN KEY REFERENCES dbo.Players(Id),
  Coach nvarchar(60) NULL, 
  Achievements nvarchar(4000) NULL
);
GO

ALTER TABLE dbo.Players
ADD CONSTRAINT FK_Players_TeamId_Teams_Id 
FOREIGN KEY (TeamId) REFERENCES dbo.Teams(Id);	 
GO
>>>>>>> master
