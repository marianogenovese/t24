//-----------------------------------------------------------------------
// <copyright file="CreateAssemblyCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create
{
    using System;
    
    /// <summary>
    /// Base class for create assemblies
    /// </summary>
    internal sealed class CreateAssemblyCommand : CreateObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new CreateAssemblyArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new CreateAssemblyDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAssemblyCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public CreateAssemblyCommand(string commandText, ISecurityContext securityContext) : base(CommandTypeEnum.CreateAssembly, commandText, securityContext)
        {
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
        /// Contains create assembly logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // implementar persistencia
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");
            Database.Repositories.Repository<Database.Models.Assembly> repo = new Database.Repositories.Repository<Database.Models.Assembly>(vc);
            Database.Models.Assembly assembly = new Database.Models.Assembly() { CreationDate = DateTime.Now, IsSystemObject = false, Type = ObjectTypeEnum.Assembly.ToString(), State = (int)UserDefinedObjectStateEnum.Stopped, Name = this.Arguments["Name"].Value.ToString(), LocalPath = this.Arguments["LocalPath"].Value.ToString() };
            repo.Create(assembly);
            repo.Commit();

            repo.Dispose();
            vc.Dispose();
        }
    }
}
