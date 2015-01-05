create view dbo.System_Streams
AS
SELECT actualObject.ObjectId as Id, actualObject.DurationTime, actualObject.UseJoin
, userDefinedObject.Name, userDefinedObject.State, userDefinedObject.IsSystemObject
, baseObject.CreationDate
FROM EngineDatabase.dbo.Streams actualObject 
INNER JOIN EngineDatabase.dbo.UserDefinedObjects userDefinedObject
ON actualObject.ObjectId = userDefinedObject.ObjectId
INNER JOIN EngineDatabase.dbo.Objects baseObject
ON actualObject.ObjectId = baseObject.ObjectId