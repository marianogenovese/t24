//-----------------------------------------------------------------------
// <copyright file="SystemQueriesCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections;
    using System.Linq;
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
                using (SystemViewsContext vc = new SystemViewsContext("EngineDatabase"))
                {
                    // get the system view name
                    string view = this.command.From.Replace(".", "_").Trim();
                    
                    // get objects
                    return new QueryCommandResult(this.GetObjects(vc, view).Cast<dynamic>().ToArray());
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Gets the objects stored in the views
        /// </summary>
        /// <param name="context">Actual context</param>
        /// <param name="view">View to query</param>
        /// <returns>Array of objects.</returns>
        private IEnumerable GetObjects(SystemViewsContext context, string view)
        {
            switch (view)
            {
                case "System_Adapters":
                    RepositoryForViews<SystemAdapter> repoAdapters = new RepositoryForViews<SystemAdapter>(context);
                    return repoAdapters.Query(this.command.Where, this.command.Select);
                case "System_Args":
                    RepositoryForViews<SystemArg> repoArgs = new RepositoryForViews<SystemArg>(context);
                    return repoArgs.Query(this.command.Where, this.command.Select);
                    
                case "System_Assemblies":
                    RepositoryForViews<SystemAssembly> repoAssembly = new RepositoryForViews<SystemAssembly>(context);
                    return repoAssembly.Query(this.command.Where, this.command.Select);
                    
                case "System_Conditions":
                    RepositoryForViews<SystemCondition> repoCondition = new RepositoryForViews<SystemCondition>(context);
                    return repoCondition.Query(this.command.Where, this.command.Select);
                    
                case "System_Dependencies":
                    RepositoryForViews<SystemDependency> repoDependency = new RepositoryForViews<SystemDependency>(context);
                    return repoDependency.Query(this.command.Where, this.command.Select);
                    
                case "System_Permissions":
                    RepositoryForViews<SystemPermission> repoPermission = new RepositoryForViews<SystemPermission>(context);
                    return repoPermission.Query(this.command.Where, this.command.Select);
                    
                case "System_PList":
                    RepositoryForViews<SystemPList> repoPList = new RepositoryForViews<SystemPList>(context);
                    return repoPList.Query(this.command.Where, this.command.Select);
                    
                case "System_Roles":
                    RepositoryForViews<SystemRole> repoRole = new RepositoryForViews<SystemRole>(context);
                    return repoRole.Query(this.command.Where, this.command.Select);
                    
                case "System_RoleMembers":
                    RepositoryForViews<SystemRoleMember> repoRoleMember = new RepositoryForViews<SystemRoleMember>(context);
                    return repoRoleMember.Query(this.command.Where, this.command.Select);
                    
                case "System_SetTrace":
                    RepositoryForViews<SystemSetTrace> repoSetTrace = new RepositoryForViews<SystemSetTrace>(context);
                    return repoSetTrace.Query(this.command.Where, this.command.Select);
                    
                case "System_Sources":
                    RepositoryForViews<SystemSource> repoSource = new RepositoryForViews<SystemSource>(context);
                    return repoSource.Query(this.command.Where, this.command.Select);
                    
                case "System_SourcesAsignedToStreams":
                    RepositoryForViews<SystemSourceAssignedToStream> repoSourceAssignedToStream = new RepositoryForViews<SystemSourceAssignedToStream>(context);
                    return repoSourceAssignedToStream.Query(this.command.Where, this.command.Select);
                    
                case "System_Stmts":
                    RepositoryForViews<SystemStmt> repoStmt = new RepositoryForViews<SystemStmt>(context);
                    return repoStmt.Query(this.command.Where, this.command.Select);
                case "System_Streams":
                    RepositoryForViews<SystemStream> repoStream = new RepositoryForViews<SystemStream>(context);
                    return repoStream.Query(this.command.Where, this.command.Select);
                case "System_Triggers":
                    RepositoryForViews<SystemTrigger> repoTrigger = new RepositoryForViews<SystemTrigger>(context);
                    return repoTrigger.Query(this.command.Where, this.command.Select);
                case "System_Users":
                    RepositoryForViews<SystemUser> repoUser = new RepositoryForViews<SystemUser>(context);
                    return repoUser.Query(this.command.Where, this.command.Select);
                case "System_UserDefinedObjects":
                    RepositoryForViews<SystemUserDefinedObject> repoUserDefinedObject = new RepositoryForViews<SystemUserDefinedObject>(context);
                    return repoUserDefinedObject.Query(this.command.Where, this.command.Select);
                default:
                    throw new Exceptions.NonExistentObjectException("The system view '" + view + "' doesn't exists.");
            }
        }
    }
}
