//-----------------------------------------------------------------------
// <copyright file="CreateRoleCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateRole
{
    using System;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for create roles
    /// </summary>
    internal sealed class CreateRoleCommand : CreateObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new CreateRoleArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new CreateRoleDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateRoleCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public CreateRoleCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.CreateRole;
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
        /// Contains create role logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");
            
            // create repository
            Database.Repositories.Repository<Database.Models.Role> repoRole = new Database.Repositories.Repository<Database.Models.Role>(vc);
            
            // create role
            Database.Models.Role role = new Database.Models.Role() { CreationDate = DateTime.Now, IsServerRole = false, IsSystemObject = false, Name = this.Arguments["Name"].Value.ToString(), State = (int)UserDefinedObjectStateEnum.Stopped, Type = ObjectTypeEnum.Role.ToString() };
            repoRole.Create(role);
            repoRole.Commit();

            // close connections
            repoRole.Dispose();
            vc.Dispose();
        }
    }
}
