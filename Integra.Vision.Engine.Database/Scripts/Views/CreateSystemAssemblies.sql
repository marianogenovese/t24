create view dbo.System_Assemblies
AS
SELECT actualObject.ObjectId as Id, actualObject.LocalPath
, userDefinedObject.Name, userDefinedObject.State, userDefinedObject.IsSystemObject
, baseObject.CreationDate
FROM EngineDatabase.dbo.Assemblies actualObject 
INNER JOIN EngineDatabase.dbo.UserDefinedObjects userDefinedObject
ON actualObject.ObjectId = userDefinedObject.ObjectId
INNER JOIN EngineDatabase.dbo.BaseObjects baseObject
ON actualObject.ObjectId = baseObject.ObjectId