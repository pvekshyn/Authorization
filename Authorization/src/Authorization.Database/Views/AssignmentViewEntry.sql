CREATE VIEW [dbo].[AssignmentViewEntry] WITH SCHEMABINDING as
SELECT a.UserId, 'User Name ' + LEFT(a.UserId, 1) as UserName, a.RoleId, r.Name as RoleName
FROM     [dbo].[Assignment] AS a INNER JOIN
                  [dbo].[Role] AS r ON a.RoleId = r.Id
