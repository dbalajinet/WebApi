-- GENERATED BY DatabaseCreator.cs
-- Copyright: Josh Comley, 2013
-- 04/09/2016 03:42:26
-- Also used in 'Start new project.linq'

USE [master]

-- Create data database
IF db_id('OData.Legacy.App.Data') IS NOT NULL
BEGIN
	ALTER DATABASE [OData.Legacy.App.Data] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE [OData.Legacy.App.Data]
END
CREATE DATABASE [OData.Legacy.App.Data]
GO

-- Create standard login
DECLARE @spidstr varchar(8000)
IF EXISTS 
    (SELECT name  
     FROM master.sys.server_principals
     WHERE name = 'antiphonalLogin')
BEGIN
	SET @spidstr = NULL
	SELECT @spidstr=coalesce(@spidstr,'' )+'kill '+convert(varchar, session_id)+ '; '
	FROM sys.dm_exec_sessions WHERE login_name = 'antiphonalLogin'
	IF LEN(@spidstr) > 0
	BEGIN
		EXEC(@spidstr)
	END

	ALTER LOGIN [antiphonalLogin] DISABLE;
	DROP LOGIN [antiphonalLogin]
END
CREATE LOGIN [antiphonalLogin] WITH PASSWORD=N'L~a]<FgjVg9>QlGO18Bp9a^^O', DEFAULT_DATABASE=[OData.Legacy.App.Data], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF

-- Create migrations login
IF EXISTS 
    (SELECT name  
     FROM master.sys.server_principals
     WHERE name = 'compliablyLogin')
BEGIN
	SET @spidstr = NULL
	SELECT @spidstr=coalesce(@spidstr,'' )+'kill '+convert(varchar, session_id)+ '; '
	FROM sys.dm_exec_sessions WHERE login_name = 'compliablyLogin'

	IF LEN(@spidstr) > 0
	BEGIN
		EXEC(@spidstr)
	END

	DROP LOGIN [compliablyLogin]
END
/* END IF NO CREATE */
CREATE LOGIN [compliablyLogin] WITH PASSWORD=N'U/EzsxrYSDpLfJBT^lbydC&qk', DEFAULT_DATABASE=[OData.Legacy.App.Data], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
--EXEC sp_addsrvrolemember @loginame = N'compliablyLogin', @rolename = N'dbcreator'

-- Create data user
USE [OData.Legacy.App.Data]
IF EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'antiphonal')
BEGIN
	DROP USER [antiphonal]
END
CREATE USER [antiphonal] FOR LOGIN [antiphonalLogin]
-- Configure standard user read/write permissions
EXEC SP_ADDROLEMEMBER DB_DATAREADER, [antiphonal]
EXEC SP_ADDROLEMEMBER DB_DATAWRITER, [antiphonal]
GO

-- Create data migrations user
USE [OData.Legacy.App.Data]
IF EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'compliably')
BEGIN
	DROP USER [compliably]
END
CREATE USER [compliably] FOR LOGIN [compliablyLogin]
EXEC SP_ADDROLEMEMBER N'db_owner', N'compliably'
GO

