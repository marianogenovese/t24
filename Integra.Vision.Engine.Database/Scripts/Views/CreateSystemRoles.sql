create view dbo.System_Roles
AS
SELECT actualObject.ObjectId as Id, actualObject.IsServerRole
, userDefinedObject.Name
, baseObject.CreationDate
FROM EngineDatabase.dbo.Roles actualObject 
INNER JOIN EngineDatabase.dbo.UserDefinedObjects userDefinedObject
ON actualObject.ObjectId = userDefinedObject.ObjectId
INNER JOIN EngineDatabase.dbo.BaseObjects baseObject
ON actualObject.ObjectId = baseObject.ObjectId