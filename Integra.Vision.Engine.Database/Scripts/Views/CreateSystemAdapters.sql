create view dbo.System_Adapters
AS
SELECT actualObject.ObjectId as Id, actualObject.Reference as Reference, actualObject.AdapterType as Type
, userDefinedObject.Name, userDefinedObject.State, userDefinedObject.IsSystemObject
, baseObject.CreationDate
FROM EngineDatabase.dbo.Adapters actualObject 
INNER JOIN EngineDatabase.dbo.UserDefinedObjects userDefinedObject
ON actualObject.ObjectId = userDefinedObject.ObjectId
INNER JOIN EngineDatabase.dbo.Objects baseObject
ON actualObject.ObjectId = baseObject.ObjectId