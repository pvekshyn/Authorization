MERGE INTO [dbo].[Role] AS [Target]
USING (VALUES 
	(@AdminRoleId)
	)
AS [Source] ([Id]) ON [Target].[Id] = [Source].[Id] 
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id]) VALUES ([Id]);
