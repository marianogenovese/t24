create view dbo.System_RoleMembers
AS
SELECT EngineDatabase.dbo.RoleMembers.RoleId as Id
, EngineDatabase.dbo.RoleMembers.UserId as Uid
, EngineDatabase.dbo.RoleMembers.CreationDate
FROM EngineDatabase.dbo.RoleMembers