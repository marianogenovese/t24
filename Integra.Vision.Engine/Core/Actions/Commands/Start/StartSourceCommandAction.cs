//-----------------------------------------------------------------------
// <copyright file="StartSourceCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;
    using Integra.Vision.Language.General;
    using Integra.Vision.Language.Runtime;

    /// <summary>
    /// Implements all the process of start a source.
    /// </summary>
    internal sealed class StartSourceCommandAction : ExecutionCommandAction
    {       
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    this.StartObject(context, command as StartSourceCommand);
                }

                return new OkCommandResult();
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
        /// <param name="command">Start source command</param>
        private void StartObject(ObjectsContext vc, StartSourceCommand command)
        {
            // create repository
            Repository<Database.Models.UserDefinedObject> repoUserDefinedObject = new Repository<Database.Models.UserDefinedObject>(vc);

            // get the adapter
            Database.Models.UserDefinedObject source = repoUserDefinedObject.Find(x => x.Name == command.Name);

            // update the adapter
            source.State = (int)UserDefinedObjectStateEnum.Started;

            // save changes
            vc.SaveChanges();

            // close connection
            vc.Dispose();
        }

        /// <summary>
        /// Contains load source logic.
        /// </summary>
        /// <param name="whereCondition">Conditional expression</param>
        private void LoadObject(string whereCondition)
        {
            System.Diagnostics.Debug.WriteLine("*** Creando función de la condicion where del source ***");
            ExpressionParser expressionParser = new ExpressionParser(whereCondition);
            PlanNode expressionNode = expressionParser.Parse();

            ExpressionConstructor constructor = new ExpressionConstructor();
            var resultWhereCondition = constructor.CompileWhere(expressionNode);
            System.Diagnostics.Debug.WriteLine("*** Función creada exitosamente ***");
        }
    }
}
