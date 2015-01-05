//-----------------------------------------------------------------------
// <copyright file="SystemQueriesCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.SystemViews
{
    using System.Linq;
    using System.Linq.Dynamic;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Models.SystemViews;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for system queries
    /// </summary>
    internal sealed class SystemQueriesCommand : PublicCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new SystemQueriesArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new SystemQueriesDependencyEnumerator();

        /// <summary>
        /// Query result
        /// </summary>
        private object[] queryResult = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemQueriesCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public SystemQueriesCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.SystemQuery;
            }
        }

        /// <summary>
        /// Gets command argument enumerator
        /// </summary>
        protected override IArgumentEnumerator ArgumentEnumerator
        {
            get
            {
                return this.argumentEnumerator;
            }
        }

        /// <summary>
        /// Gets command dependency enumerator
        /// </summary>
        protected override IDependencyEnumerator DependencyEnumerator
        {
            get
            {
                return this.dependencyEnumerator;
            }
        }

        /// <summary>
        /// Contains stop object logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // initialize the context
            SystemViewsContext vc = new SystemViewsContext("EngineDatabase");

            // get the system view name
            string view = this.Arguments["from"].Value.ToString().Replace(".", "_").Trim();

            // get objects
            this.GetObjects(vc, view);

            // close connection
            vc.Dispose();
        }

        /// <summary>
        /// Gets the objects stored in the views
        /// </summary>
        /// <param name="context">actual context</param>
        /// <param name="view">view to query</param>
        protected void GetObjects(SystemViewsContext context, string view)
        {
            switch (view)
            {
                case "System_Adapters":
                    RepositoryForViews<SystemAdapter> repoAdapters = new RepositoryForViews<SystemAdapter>(context);
                    this.queryResult = repoAdapters.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoAdapters.Dispose();
                    break;
                case "System_Args":
                    RepositoryForViews<SystemArg> repoArgs = new RepositoryForViews<SystemArg>(context);
                    this.queryResult = repoArgs.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoArgs.Dispose();
                    break;
                case "System_Assemblies":
                    RepositoryForViews<SystemAssembly> repoAssembly = new RepositoryForViews<SystemAssembly>(context);
                    this.queryResult = repoAssembly.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoAssembly.Dispose();
                    break;
                case "System_Conditions":
                    RepositoryForViews<SystemCondition> repoCondition = new RepositoryForViews<SystemCondition>(context);
                    this.queryResult = repoCondition.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoCondition.Dispose();
                    break;
                case "System_Dependencies":
                    RepositoryForViews<SystemDependency> repoDependency = new RepositoryForViews<SystemDependency>(context);
                    this.queryResult = repoDependency.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoDependency.Dispose();
                    break;
                case "System_Permissions":
                    RepositoryForViews<SystemPermission> repoPermission = new RepositoryForViews<SystemPermission>(context);
                    this.queryResult = repoPermission.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoPermission.Dispose();
                    break;
                case "System_PList":
                    RepositoryForViews<SystemPList> repoPList = new RepositoryForViews<SystemPList>(context);
                    this.queryResult = repoPList.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoPList.Dispose();
                    break;
                case "System_Roles":
                    RepositoryForViews<SystemRole> repoRole = new RepositoryForViews<SystemRole>(context);
                    this.queryResult = repoRole.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoRole.Dispose();
                    break;
                case "System_RoleMembers":
                    RepositoryForViews<SystemRoleMember> repoRoleMember = new RepositoryForViews<SystemRoleMember>(context);
                    this.queryResult = repoRoleMember.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoRoleMember.Dispose();
                    break;
                case "System_SetTrace":
                    RepositoryForViews<SystemSetTrace> repoSetTrace = new RepositoryForViews<SystemSetTrace>(context);
                    this.queryResult = repoSetTrace.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoSetTrace.Dispose();
                    break;
                case "System_Sources":
                    RepositoryForViews<SystemSource> repoSource = new RepositoryForViews<SystemSource>(context);
                    this.queryResult = repoSource.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoSource.Dispose();
                    break;
                case "System_SourcesAsignedToStreams":
                    RepositoryForViews<SystemSourceAssignedToStream> repoSourceAssignedToStream = new RepositoryForViews<SystemSourceAssignedToStream>(context);
                    this.queryResult = repoSourceAssignedToStream.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoSourceAssignedToStream.Dispose();
                    break;
                case "System_Stmts":
                    RepositoryForViews<SystemStmt> repoStmt = new RepositoryForViews<SystemStmt>(context);
                    this.queryResult = repoStmt.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoStmt.Dispose();
                    break;
                case "System_Streams":
                    RepositoryForViews<SystemStream> repoStream = new RepositoryForViews<SystemStream>(context);
                    this.queryResult = repoStream.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoStream.Dispose();
                    break;
                case "System_Triggers":
                    RepositoryForViews<SystemTrigger> repoTrigger = new RepositoryForViews<SystemTrigger>(context);
                    this.queryResult = repoTrigger.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoTrigger.Dispose();
                    break;
                case "System_Users":
                    RepositoryForViews<SystemUser> repoUser = new RepositoryForViews<SystemUser>(context);
                    this.queryResult = repoUser.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoUser.Dispose();
                    break;
                case "System_UserDefinedObjects":
                    RepositoryForViews<SystemUserDefinedObject> repoUserDefinedObject = new RepositoryForViews<SystemUserDefinedObject>(context);
                    this.queryResult = repoUserDefinedObject.Query(this.Arguments["where"].Value.ToString(), this.Arguments["select"].Value.ToString());
                    repoUserDefinedObject.Dispose();
                    break;
                default:
                    throw new Exceptions.NonExistentObjectException("The system view '" + view + "' doesn't exists.");
            }
        }
    }
}
