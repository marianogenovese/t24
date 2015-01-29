//-----------------------------------------------------------------------
// <copyright file="CreateSourceCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of create a new source.
    /// </summary>
    internal sealed class CreateSourceCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    this.SaveArguments(context, command as CreateSourceCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Save the source command arguments
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Create source command</param>
        private void SaveArguments(ObjectsContext vc, CreateSourceCommand command)
        {
            // create the source
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);
            Database.Models.Source source = new Database.Models.Source() { CreationDate = DateTime.Now, IsSystemObject = false, Name = command.Name, State = (int)UserDefinedObjectStateEnum.Stopped, Type = ObjectTypeEnum.Source.ToString() };
            repoSource.Create(source);

            // create the conditions
            Database.Repositories.Repository<Database.Models.SourceCondition> repoSourceCondition = new Database.Repositories.Repository<Database.Models.SourceCondition>(vc);
            Database.Models.SourceCondition sourceCondition = new Database.Models.SourceCondition() { Expression = command.Where, SourceId = source.Id, Type = (int)ConditionTypeEnum.FilterCondition };
            repoSourceCondition.Create(sourceCondition);

            // save the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.SaveScript(command.Script, source.Id);

            // save dependencies of the source
            DependencyActions dependencyAction = new DependencyActions(vc, command.Dependencies);
            dependencyAction.SaveDependencies(source);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
