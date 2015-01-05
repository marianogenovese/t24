create view dbo.System_Users
AS
SELECT actualObject.ObjectId as Id, actualObject.SId as Sid, actualObject.Password
, userDefinedObject.State as Status
, baseObject.CreationDate
FROM EngineDatabase.dbo.Users actualObject 
INNER JOIN EngineDatabase.dbo.UserDefinedObjects userDefinedObject
ON actualObject.ObjectId = userDefinedObject.ObjectId
INNER JOIN EngineDatabase.dbo.Objects baseObject
ON actualObject.ObjectId = baseObject.ObjectId