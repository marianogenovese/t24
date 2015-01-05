create view dbo.System_Dependencies
AS
SELECT actualObject.PrincipalObjectId as Id, actualObject.DependencyObjectId as DependenceId
, baseObject.CreationDate
FROM EngineDatabase.dbo.Dependencies actualObject 
INNER JOIN EngineDatabase.dbo.UserDefinedObjects userDefinedObject
ON actualObject.PrincipalObjectId = userDefinedObject.ObjectId
INNER JOIN EngineDatabase.dbo.Objects baseObject
ON actualObject.PrincipalObjectId = baseObject.ObjectId