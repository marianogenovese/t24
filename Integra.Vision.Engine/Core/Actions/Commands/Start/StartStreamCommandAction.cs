//-----------------------------------------------------------------------
// <copyright file="StartStreamCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Linq;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;
    using Integra.Vision.Language.General;
    using Integra.Vision.Language.Runtime;

    /// <summary>
    /// Implements all the process of start a stream.
    /// </summary>
    internal sealed class StartStreamCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    this.StartObject(context, command as StartStreamCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains start object logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Start stream command</param>
        private void StartObject(ViewsContext vc, StartStreamCommand command)
        {
            // create repository
            Repository<Database.Models.UserDefinedObject> repoUserDefinedObject = new Repository<Database.Models.UserDefinedObject>(vc);
            Repository<Database.Models.PList> repoProjection = new Repository<Database.Models.PList>(vc);
            Repository<Database.Models.StreamCondition> repoStreamConditions = new Repository<Database.Models.StreamCondition>(vc);

            // get the adapter
            Database.Models.UserDefinedObject stream = repoUserDefinedObject.Find(x => x.Name == command.Name);

            // update the adapter
            stream.State = (int)UserDefinedObjectStateEnum.Started;

            // get the stream projection
            IQueryable projection = repoProjection.Filter(x => x.StreamId == stream.Id);
            string projectionScript = this.GenerateProjectionScript(projection);

            // get the stream conditions
            IQueryable conditions = repoStreamConditions.Filter(x => x.StreamId == stream.Id);
            string onCondition = string.Empty;
            string whereCondition = string.Empty;
            foreach (Database.Models.StreamCondition condition in conditions)
            {
                if (condition.IsOnCondition)
                {
                    onCondition = condition.Expression;
                }
                else
                {
                    whereCondition = condition.Expression;
                }
            }

            // load object
            if (onCondition.Equals(string.Empty))
            {
                this.LoadObject(whereCondition, projectionScript);
            }
            else
            {
                this.LoadObject(onCondition, whereCondition, projectionScript);
            }

            // save changes
            vc.SaveChanges();

            // close connection
            vc.Dispose();
        }

        /// <summary>
        /// Contains load stream logic.
        /// </summary>
        /// <param name="onCondition">On condition expression</param>
        /// <param name="whereCondition">Where condition expression</param>
        /// <param name="projection">Projection expression</param>
        private void LoadObject(string onCondition, string whereCondition, string projection)
        {
            System.Diagnostics.Debug.WriteLine("*** Iniciará start del stream ***");
            ProjectionParser projectionParser = new ProjectionParser(projection);
            PlanNode projectionNode = projectionParser.Parse();

            ExpressionParser onConditionParser = new ExpressionParser(onCondition);
            PlanNode onConditionNode = onConditionParser.Parse();

            ExpressionParser whereConditionParser = new ExpressionParser(whereCondition);
            PlanNode whereConditionNode = whereConditionParser.Parse();

            ExpressionConstructor constructor = new ExpressionConstructor();
            var resultProjection = constructor.CompileJoinSelect(projectionNode);
            constructor = new ExpressionConstructor();
            var resultOnCondition = constructor.CompileJoinWhere(onConditionNode);
            constructor = new ExpressionConstructor();
            var resultWhereCondition = constructor.CompileJoinWhere(whereConditionNode);
            System.Diagnostics.Debug.WriteLine("*** Terminó start del stream ***");
        }

        /// <summary>
        /// Contains load stream logic.
        /// </summary>
        /// <param name="whereCondition">Where condition expression</param>
        /// <param name="projection">Projection expression</param>
        private void LoadObject(string whereCondition, string projection)
        {
            System.Diagnostics.Debug.WriteLine("*** Iniciará start del stream ***");
            ProjectionParser projectionParser = new ProjectionParser(projection);
            PlanNode projectionNode = projectionParser.Parse();

            ExpressionParser whereConditionParser = new ExpressionParser(whereCondition);
            PlanNode whereConditionNode = whereConditionParser.Parse();

            ExpressionConstructor constructor = new ExpressionConstructor();
            var resultProjection = constructor.CompileSelect(projectionNode);
            constructor = new ExpressionConstructor();
            var resultWhereCondition = constructor.CompileWhere(whereConditionNode);
            System.Diagnostics.Debug.WriteLine("*** Terminó start del stream ***");
        }

        /// <summary>
        /// Generate the projection script of the stream
        /// </summary>
        /// <param name="projection">Projection arguments</param>
        /// <returns>Projection script</returns>
        private string GenerateProjectionScript(IQueryable projection)
        {
            string script = "select ";
            bool isFirst = true;

            foreach (Database.Models.PList tupla in projection)
            {
                if (isFirst)
                {
                    script += tupla.Expression + " as " + tupla.Alias;
                    isFirst = false;
                }
                else
                {
                    script += ", " + tupla.Expression + " as " + tupla.Alias;
                }
            }

            return script;
        }
    }
}
