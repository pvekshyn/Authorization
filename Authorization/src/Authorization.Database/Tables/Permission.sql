CREATE TABLE [dbo].[Permission]
(
	[Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(50) NOT NULL,
	CONSTRAINT [PK_Permission] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_PermissionName] UNIQUE([Name]),
)
