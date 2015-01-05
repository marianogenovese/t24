create view dbo.System_Args
AS
SELECT EngineDatabase.dbo.Args.AdapterId as Id
, EngineDatabase.dbo.Args.Type
, EngineDatabase.dbo.Args.Name 
, EngineDatabase.dbo.Args.Value
FROM EngineDatabase.dbo.Args