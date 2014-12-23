﻿//-----------------------------------------------------------------------
// <copyright file="DropUserCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Drop.DropUser
{
    using System;

    /// <summary>
    /// Base class for drop users
    /// </summary>
    internal class DropUserCommand : DropObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new DropUserArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new DropUserDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="DropUserCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public DropUserCommand(string commandText, ISecurityContext securityContext) : base(CommandTypeEnum.DropUser, commandText, securityContext)
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
        /// Delete the user arguments
        /// </summary>
        public virtual void DeleteArguments()
        {
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.User> repo = new Database.Repositories.Repository<Database.Models.User>(vc);

            // get the object name
            string objectName = this.Arguments["Name"].Value.ToString();

            // delete the object
            repo.Delete(x => x.Name == objectName);
            int objectCount = repo.Commit();

            // throw an exception if not deleted a object
            if (objectCount != 1)
            {
                // close connection
                repo.Dispose();
                vc.Dispose();

                // throw the exception 
                throw new Integra.Vision.Engine.Exceptions.DropUserDefinedObjectException("The object '" + objectName + "' was not removed");
            }

            // close connection
            repo.Dispose();
            vc.Dispose();
        }

        /// <summary>
        /// Contains drop user logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // delete arguments
            this.DeleteArguments();
        }
    }
}
