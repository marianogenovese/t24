create view dbo.System_Conditions
AS
select EngineDatabase.dbo.StreamConditions.StreamId as Id, EngineDatabase.dbo.StreamConditions.Type, EngineDatabase.dbo.StreamConditions.Expression, EngineDatabase.dbo.StreamConditions.IsOnCondition as IsOnCondition 
from EngineDatabase.dbo.StreamConditions