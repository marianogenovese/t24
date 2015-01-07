//-----------------------------------------------------------------------
// <copyright file="SystemQueriesCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Models.SystemViews;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of the queries system views.
    /// </summary>
    internal sealed class SystemQueriesCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Query result
        /// </summary>
        private object[] queryResult = null;

        /// <summary>
        /// Create adapter command
        /// </summary>
        private SystemQueriesCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as SystemQueriesCommand;

            try
            {
                // initialize the context
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");

                // get the system view name
                string view = this.command.From.Replace(".", "_").Trim();

                // get objects
                this.GetObjects(vc, view);

                // close connection
                vc.Dispose();

                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Gets the objects stored in the views
        /// </summary>
        /// <param name="context">actual context</param>
        /// <param name="view">view to query</param>
        private void GetObjects(SystemViewsContext context, string view)
        {
            switch (view)
            {
                case "System_Adapters":
                    RepositoryForViews<SystemAdapter> repoAdapters = new RepositoryForViews<SystemAdapter>(context);
                    this.queryResult = repoAdapters.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Args":
                    RepositoryForViews<SystemArg> repoArgs = new RepositoryForViews<SystemArg>(context);
                    this.queryResult = repoArgs.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Assemblies":
                    RepositoryForViews<SystemAssembly> repoAssembly = new RepositoryForViews<SystemAssembly>(context);
                    this.queryResult = repoAssembly.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Conditions":
                    RepositoryForViews<SystemCondition> repoCondition = new RepositoryForViews<SystemCondition>(context);
                    this.queryResult = repoCondition.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Dependencies":
                    RepositoryForViews<SystemDependency> repoDependency = new RepositoryForViews<SystemDependency>(context);
                    this.queryResult = repoDependency.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Permissions":
                    RepositoryForViews<SystemPermission> repoPermission = new RepositoryForViews<SystemPermission>(context);
                    this.queryResult = repoPermission.Query(this.command.Where, this.command.Select);
                    break;
                case "System_PList":
                    RepositoryForViews<SystemPList> repoPList = new RepositoryForViews<SystemPList>(context);
                    this.queryResult = repoPList.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Roles":
                    RepositoryForViews<SystemRole> repoRole = new RepositoryForViews<SystemRole>(context);
                    this.queryResult = repoRole.Query(this.command.Where, this.command.Select);
                    break;
                case "System_RoleMembers":
                    RepositoryForViews<SystemRoleMember> repoRoleMember = new RepositoryForViews<SystemRoleMember>(context);
                    this.queryResult = repoRoleMember.Query(this.command.Where, this.command.Select);
                    break;
                case "System_SetTrace":
                    RepositoryForViews<SystemSetTrace> repoSetTrace = new RepositoryForViews<SystemSetTrace>(context);
                    this.queryResult = repoSetTrace.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Sources":
                    RepositoryForViews<SystemSource> repoSource = new RepositoryForViews<SystemSource>(context);
                    this.queryResult = repoSource.Query(this.command.Where, this.command.Select);
                    break;
                case "System_SourcesAsignedToStreams":
                    RepositoryForViews<SystemSourceAssignedToStream> repoSourceAssignedToStream = new RepositoryForViews<SystemSourceAssignedToStream>(context);
                    this.queryResult = repoSourceAssignedToStream.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Stmts":
                    RepositoryForViews<SystemStmt> repoStmt = new RepositoryForViews<SystemStmt>(context);
                    this.queryResult = repoStmt.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Streams":
                    RepositoryForViews<SystemStream> repoStream = new RepositoryForViews<SystemStream>(context);
                    this.queryResult = repoStream.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Triggers":
                    RepositoryForViews<SystemTrigger> repoTrigger = new RepositoryForViews<SystemTrigger>(context);
                    this.queryResult = repoTrigger.Query(this.command.Where, this.command.Select);
                    break;
                case "System_Users":
                    RepositoryForViews<SystemUser> repoUser = new RepositoryForViews<SystemUser>(context);
                    this.queryResult = repoUser.Query(this.command.Where, this.command.Select);
                    break;
                case "System_UserDefinedObjects":
                    RepositoryForViews<SystemUserDefinedObject> repoUserDefinedObject = new RepositoryForViews<SystemUserDefinedObject>(context);
                    this.queryResult = repoUserDefinedObject.Query(this.command.Where, this.command.Select);
                    break;
                default:
                    throw new Exceptions.NonExistentObjectException("The system view '" + view + "' doesn't exists.");
            }
        }
    }
}
