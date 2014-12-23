create view dbo.System_Triggers
AS
SELECT actualObject.ObjectId as Id, actualObject.DurationTime, actualObject.StreamId
, userDefinedObject.Name, userDefinedObject.State, userDefinedObject.IsSystemObject
, baseObject.CreationDate
FROM EngineDatabase.dbo.Triggers actualObject 
INNER JOIN EngineDatabase.dbo.UserDefinedObjects userDefinedObject
ON actualObject.ObjectId = userDefinedObject.ObjectId
INNER JOIN EngineDatabase.dbo.Objects baseObject
ON actualObject.ObjectId = baseObject.ObjectId