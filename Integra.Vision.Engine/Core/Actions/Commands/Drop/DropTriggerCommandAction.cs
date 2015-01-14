//-----------------------------------------------------------------------
// <copyright file="DropTriggerCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of delete a trigger.
    /// </summary>
    internal sealed class DropTriggerCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    this.DeleteObject(context, command as DropTriggerCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains drop trigger logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Drop trigger command</param>
        private void DeleteObject(ViewsContext vc, DropTriggerCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.Trigger> repoTrigger = new Database.Repositories.Repository<Database.Models.Trigger>(vc);
            Database.Repositories.Repository<Database.Models.Stmt> repoStmt = new Database.Repositories.Repository<Database.Models.Stmt>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            // get the trigger
            Database.Models.Trigger trigger = repoTrigger.Find(x => x.Name == command.Name);

            // delete the statements
            repoStmt.Delete(x => x.TriggerId == trigger.Id);
            /*
            int stmtCount = repoStmt.Commit();

            // throw an exception if not deleted a statement
            if (stmtCount < 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The trigger statements were not removed");
            }
            */

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == trigger.Id);
            /*
            int dependencyCount = repoDependency.Commit();

            // throw an exception if not deleted a dependency
            if (dependencyCount < 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The dependencies were not removed");
            }
            */

            // delete the object
            repoTrigger.Delete(x => x.Name == command.Name);
            int objectCount = vc.SaveChanges();

            // throw an exception if not deleted a object
            if (objectCount < 1)
            {
                // close connection
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The object '" + command.Name + "' was not removed");
            }

            // close connection
            vc.Dispose();
        }
    }
}
