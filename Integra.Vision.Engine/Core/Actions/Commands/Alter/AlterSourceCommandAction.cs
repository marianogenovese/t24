//-----------------------------------------------------------------------
// <copyright file="AlterSourceCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of alter a source.
    /// </summary>
    internal sealed class AlterSourceCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    this.UpdateObject(context, command as AlterSourceCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains alter source logic.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Alter source command</param>
        private void UpdateObject(ViewsContext vc, AlterSourceCommand command)
        {
            Database.Repositories.Repository<Database.Models.Adapter> repoAdapter = new Database.Repositories.Repository<Database.Models.Adapter>(vc);
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);
            Database.Repositories.Repository<Database.Models.SourceCondition> repoSourceCondition = new Database.Repositories.Repository<Database.Models.SourceCondition>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            // get the adapter
            Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == command.From);

            // get the source            
            Database.Models.Source source = repoSource.Find(x => x.Name == command.Name);

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == source.Id);

            // detele the conditions
            repoSourceCondition.Delete(x => x.SourceId == source.Id);

            // update the source arguments
            source.AdapterId = adapter.Id;
            source.CreationDate = DateTime.Now;
            source.IsSystemObject = false;
            source.State = (int)UserDefinedObjectStateEnum.Stopped;
            source.Type = ObjectTypeEnum.Source.ToString();

            // update the source
            repoSource.Update(source);

            // create the conditions            
            Database.Models.SourceCondition sourceCondition = new Database.Models.SourceCondition() { Expression = command.Where, SourceId = source.Id, Type = (int)ConditionTypeEnum.FilterCondition };
            repoSourceCondition.Create(sourceCondition);

            // update the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.UpdateScript(command.Script, source.Id);

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
