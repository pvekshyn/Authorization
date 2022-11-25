CREATE LOGIN [authorization_service] 
WITH PASSWORD = 'authorization_password',
DEFAULT_DATABASE = [authorization],
CHECK_POLICY     = OFF,
CHECK_EXPIRATION = OFF;

GO
CREATE USER [authorization_service]
        FOR LOGIN [authorization_service]
        WITH DEFAULT_SCHEMA = dbo
GO
EXEC sp_addrolemember N'db_owner', N'authorization_service'
