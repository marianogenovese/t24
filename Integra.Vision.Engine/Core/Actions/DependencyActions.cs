//-----------------------------------------------------------------------
// <copyright file="DependencyActions.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Class that contains the object dependency actions
    /// </summary>
    internal sealed class DependencyActions
    {
        /// <summary>
        /// Actual context
        /// </summary>
        private ObjectsContext context;

        /// <summary>
        /// Dependencies of the actual object
        /// </summary>
        private IReadOnlyNamedElementCollection<CommandDependency> dependencies;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyActions"/> class
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="dependencies">Object dependencies</param>
        public DependencyActions(ObjectsContext context, IReadOnlyNamedElementCollection<CommandDependency> dependencies)
        {
            this.dependencies = dependencies;
            this.context = context;
        }

        /// <summary>
        /// Save the dependencies of the actual object
        /// </summary>
        /// <param name="userDefinedObject">the actual adapter</param>
        public void SaveDependencies(Database.Models.UserDefinedObject userDefinedObject)
        {
            Repository<Database.Models.Dependency> repoDependency = new Repository<Database.Models.Dependency>(this.context);
            Repository<Database.Models.UserDefinedObject> repoSource = new Repository<Database.Models.UserDefinedObject>(this.context);

            foreach (var dependency in this.dependencies)
            {
                Database.Models.UserDefinedObject dependencyObject = repoSource.Find(x => x.Name == dependency.Name);

                if (dependencyObject == null)
                {
                    throw new Integra.Vision.Engine.Exceptions.NonExistentObjectException("The dependency '" + dependency.Name + "' does not exist");
                }

                Database.Models.Dependency newDependency = new Database.Models.Dependency() { DependencyObjectId = dependencyObject.Id, PrincipalObjectId = userDefinedObject.Id };
                repoDependency.Create(newDependency);
            }
        }

        /// <summary>
        /// Doc goes here
        /// </summary>
        /// <param name="name">Dependency name</param>
        /// <returns>True if the dependency exists</returns>
        protected bool ExistsDependency(string name)
        {
            throw new NotImplementedException();
        }
    }
}
