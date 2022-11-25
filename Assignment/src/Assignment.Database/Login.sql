CREATE LOGIN [assignment_service] 
WITH PASSWORD = 'assignment_password',
DEFAULT_DATABASE = [Assignment],
CHECK_POLICY     = OFF,
CHECK_EXPIRATION = OFF;

GO
CREATE USER [assignment_service]
        FOR LOGIN [assignment_service]
        WITH DEFAULT_SCHEMA = dbo
GO
EXEC sp_addrolemember N'db_owner', N'assignment_service'
