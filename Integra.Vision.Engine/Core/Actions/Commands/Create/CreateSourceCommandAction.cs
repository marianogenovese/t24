//-----------------------------------------------------------------------
// <copyright file="CreateSourceCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of create a new source.
    /// </summary>
    internal sealed class CreateSourceCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Create adapter command
        /// </summary>
        private CreateSourceCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as CreateSourceCommand;

            try
            {
                this.SaveArguments();
                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// save the command arguments
        /// </summary>
        private void SaveArguments()
        {
            // Initialize the context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // get the adapter
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);
            Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == this.command.From);

            if (adapter == null)
            {
                throw new Integra.Vision.Engine.Exceptions.NonExistentObjectException("The adapter '" + this.command.From + "' does not exist");
            }

            // create the source
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);
            Database.Models.Source source = new Database.Models.Source() { AdapterId = adapter.Id, CreationDate = DateTime.Now, IsSystemObject = false, Name = this.command.Name, State = (int)UserDefinedObjectStateEnum.Stopped, Type = ObjectTypeEnum.Source.ToString() };
            repoSource.Create(source);

            // create the conditions
            Database.Repositories.Repository<Database.Models.SourceCondition> repoSourceCondition = new Database.Repositories.Repository<Database.Models.SourceCondition>(vc);
            Database.Models.SourceCondition sourceCondition = new Database.Models.SourceCondition() { Expression = this.command.Where, SourceId = source.Id, Type = (int)ConditionTypeEnum.FilterCondition };
            repoSourceCondition.Create(sourceCondition);
            
            // save dependencies of the source
            DependencyActions dependencyAction = new DependencyActions(vc, this.command.Dependencies);
            dependencyAction.SaveDependencies(source);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
