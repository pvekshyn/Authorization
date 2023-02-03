CREATE LOGIN [role_service] 
WITH PASSWORD = 'SecurePassword#123',
DEFAULT_DATABASE = [Role],
CHECK_POLICY     = OFF,
CHECK_EXPIRATION = OFF;

GO
CREATE USER [role_service]
        FOR LOGIN [role_service]
        WITH DEFAULT_SCHEMA = dbo
GO
EXEC sp_addrolemember N'db_owner', N'role_service'
