CREATE TABLE [dbo].[OutboxMessageError]
(
	[Id] uniqueidentifier NOT NULL default newid(),
    [OutboxMessageId] uniqueidentifier NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [StackTrace] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL,
	CONSTRAINT [PK_OutboxMessageError] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OutboxMessageError_OutboxMessage_OutboxMessageId] FOREIGN KEY ([OutboxMessageId]) REFERENCES [OutboxMessage] ([Id]) ON DELETE CASCADE,
    INDEX IX_OutboxMessageId NONCLUSTERED (OutboxMessageId)
)