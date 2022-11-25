CREATE TABLE [dbo].[InboxMessage]
(
	[Id] uniqueidentifier NOT NULL default newid(),
    [Message] nvarchar(max) NOT NULL,
    [Created] datetime2 NOT NULL,
	CONSTRAINT [PK_InboxMessage] PRIMARY KEY ([Id]),
	INDEX IX_Created NONCLUSTERED (Created)
)
