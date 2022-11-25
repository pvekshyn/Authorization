CREATE TABLE [dbo].[Role]
(
	[Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(25) NOT NULL,
	CONSTRAINT [PK_Role] PRIMARY KEY ([Id]),
	CONSTRAINT [UQ_RoleName] UNIQUE([Name]),
)
