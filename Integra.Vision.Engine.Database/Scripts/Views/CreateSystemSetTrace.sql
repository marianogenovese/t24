create view dbo.System_SetTrace
AS
SELECT EngineDatabase.dbo.SetTraces.UserDefinedObjectId as Id
, EngineDatabase.dbo.SetTraces.Level
, EngineDatabase.dbo.SetTraces.CreationDate
FROM EngineDatabase.dbo.SetTraces