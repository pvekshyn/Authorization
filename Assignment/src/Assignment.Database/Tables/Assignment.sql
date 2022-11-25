CREATE TABLE [Assignment] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Assignment] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_UserId_RoleId] UNIQUE(UserId, RoleId),
    CONSTRAINT [FK_Assignment_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]) ON DELETE CASCADE
    ) 