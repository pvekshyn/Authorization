MERGE INTO [dbo].[Assignment] AS [Target]
USING (VALUES 
	(@AdminUserId, @AdminRoleId)
	)
AS [Source] ([UserId], [RoleId]) ON [Target].[UserId] = [Source].[UserId] AND [Target].[RoleId] = [Source].[RoleId]
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [UserId], [RoleId]) VALUES (NEWID(), [UserId], [RoleId]);
