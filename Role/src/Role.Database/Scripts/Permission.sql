MERGE INTO [dbo].[Permission] AS [Target]
USING (VALUES 
	(@CreatePermission, 'Create Permission'),
	(@ReadPermission, 'Read Permission'),
	(@EditPermission, 'Edit Permission'),
	(@DeletePermission, 'Delete Permission'),
	(@CreateRole, 'Create Role'),
	(@ReadRole, 'Read Role'),
	(@EditRole, 'Edit Role'),
	(@DeleteRole, 'Delete Role'),
	(@Assign, 'Assign'),
	(@Deassign, 'Deassign')
	)
AS [Source] ([Id], [Name]) ON [Target].[Id] = [Source].[Id] 
WHEN MATCHED THEN 
	UPDATE SET [Name] = [Source].[Name]
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name]) VALUES ([Id], [Name]);