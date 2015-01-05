create view dbo.System_UserDefinedObjects
AS
SELECT EngineDatabase.dbo.UserDefinedObjects.ObjectId as Id
, EngineDatabase.dbo.UserDefinedObjects.Name
, EngineDatabase.dbo.UserDefinedObjects.State
, EngineDatabase.dbo.UserDefinedObjects.IsSystemObject
FROM EngineDatabase.dbo.UserDefinedObjects