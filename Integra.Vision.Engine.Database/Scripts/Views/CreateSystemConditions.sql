create view dbo.System_Conditions
AS
select EngineDatabase.dbo.SourceConditions.SourceId as Id, EngineDatabase.dbo.SourceConditions.Type, EngineDatabase.dbo.SourceConditions.Expression, CONVERT(bit,0) as IsOnCondition 
from EngineDatabase.dbo.SourceConditions
union
select EngineDatabase.dbo.StreamConditions.StreamId as Id, EngineDatabase.dbo.StreamConditions.Type, EngineDatabase.dbo.StreamConditions.Expression, EngineDatabase.dbo.StreamConditions.IsOnCondition as IsOnCondition 
from EngineDatabase.dbo.StreamConditions