create view dbo.System_Stmts
AS
SELECT EngineDatabase.dbo.Stmts.TriggerId as Id
, EngineDatabase.dbo.Stmts.[Order]
, EngineDatabase.dbo.Stmts.Type
, EngineDatabase.dbo.Stmts.AdapterId
FROM EngineDatabase.dbo.Stmts