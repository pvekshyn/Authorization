CREATE TABLE [dbo].[Assignment]
(
	[Id] uniqueidentifier NOT NULL,
	[UserId] uniqueidentifier NOT NULL,
	[RoleId] uniqueidentifier NOT NULL,
	CONSTRAINT [PK_Assignment] PRIMARY KEY ([Id]),
	CONSTRAINT [UQ_UserId_RoleId] UNIQUE(UserId, RoleId)
)
