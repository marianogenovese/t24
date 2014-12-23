//-----------------------------------------------------------------------
// <copyright file="CommandDependencyCollection.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    // TODO Doc
    
    /// <summary>
    /// CommandDependencyCollection
    /// Doc goes here
    /// </summary>
    internal sealed class CommandDependencyCollection : ReadOnlyNamedElementCollectionBase<CommandDependency>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDependencyCollection"/> class
        /// </summary>
        /// <param name="dependencies">dependencies doc goes here</param>
        public CommandDependencyCollection(CommandDependency[] dependencies) : base(dependencies)
        {
        }
    }
}
