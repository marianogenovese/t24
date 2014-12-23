create view dbo.System_PList
AS
SELECT EngineDatabase.dbo.PLists.StreamId as Id
, EngineDatabase.dbo.PLists.Expression
, EngineDatabase.dbo.PLists.Alias
, EngineDatabase.dbo.PLists.[Order]
FROM EngineDatabase.dbo.PLists