MERGE INTO [dbo].[Role] AS [Target]
USING (VALUES 
	(@AdminRoleId, 'Admin')
	)
AS [Source] ([Id], [Name]) ON [Target].[Id] = [Source].[Id] 
WHEN MATCHED THEN 
	UPDATE SET [Name] = [Source].[Name]
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name]) VALUES ([Id], [Name]);

MERGE INTO [dbo].[RolePermission] AS [Target]
USING (
     SELECT NEWID(), @AdminRoleId, p.Id
     FROM [dbo].[Permission] p
    )
AS [Source] ([Id], [RoleId], [PermissionId]) ON [Target].[RoleId] = [Source].[RoleId] AND [Target].[PermissionId] = [Source].[PermissionId]
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [RoleId], [Permissionid]) VALUES ([Id], [RoleId], [Permissionid]);