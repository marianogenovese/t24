create view dbo.System_Sources
AS
SELECT actualObject.ObjectId as Id, actualObject.AdapterId
, userDefinedObject.Name, userDefinedObject.State, userDefinedObject.IsSystemObject
, baseObject.CreationDate
FROM EngineDatabase.dbo.Sources actualObject 
INNER JOIN EngineDatabase.dbo.UserDefinedObjects userDefinedObject
ON actualObject.ObjectId = userDefinedObject.ObjectId
INNER JOIN EngineDatabase.dbo.Objects baseObject
ON actualObject.ObjectId = baseObject.ObjectId