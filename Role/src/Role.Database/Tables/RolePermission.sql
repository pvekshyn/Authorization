CREATE TABLE [RolePermission] (
    [Id] uniqueidentifier NOT NULL default newid(),
    [RoleId] uniqueidentifier NOT NULL,
    [PermissionId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_RolePermission] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RolePermission_Permission_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permission] ([Id]),
    CONSTRAINT [FK_RolePermission_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_RoleId_PermissionId] UNIQUE(RoleId, PermissionId)
)