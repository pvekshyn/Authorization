CREATE TABLE [dbo].[OutboxMessage]
(
	[Id] uniqueidentifier NOT NULL default newid(),
    [EntityId] uniqueidentifier NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [Type] int NOT NULL,
    [Created] datetime2 NOT NULL,
	CONSTRAINT [PK_OutboxMessage] PRIMARY KEY ([Id])
)