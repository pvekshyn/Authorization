CREATE TABLE [dbo].[InboxMessageError]
(
	[Id] uniqueidentifier NOT NULL default newid(),
    [InboxMessageId] uniqueidentifier NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [StackTrace] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL,
	CONSTRAINT [PK_InboxMessageError] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InboxMessageError_InboxMessage_InboxMessageId] FOREIGN KEY ([InboxMessageId]) REFERENCES [InboxMessage] ([Id]) ON DELETE CASCADE,
)