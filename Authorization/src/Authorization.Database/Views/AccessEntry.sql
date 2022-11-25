CREATE VIEW [dbo].[AccessEntry] WITH SCHEMABINDING as
SELECT a.UserId, a.RoleId, p.PermissionId
FROM     [dbo].[Assignment] AS a INNER JOIN
                  [dbo].[RolePermission] AS p ON a.RoleId = p.RoleId
