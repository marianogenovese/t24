create view dbo.System_Permissions
AS
SELECT permission.RoleId as Sid, permission.UserDefinedObjectId as Oid, permission.Type, permission.CreationDate
FROM EngineDatabase.dbo.PermissionRoles as permission
UNION
SELECT permission.UserId as Sid, permission.UserDefinedObjectId as Oid, permission.Type, permission.CreationDate
FROM EngineDatabase.dbo.PermissionUsers as permission