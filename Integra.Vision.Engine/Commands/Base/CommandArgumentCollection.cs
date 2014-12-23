//-----------------------------------------------------------------------
// <copyright file="CommandArgumentCollection.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    // TODO Doc
    
    /// <summary>
    /// CommandArgumentCollection
    /// Doc goes here
    /// </summary>
    internal sealed class CommandArgumentCollection : ReadOnlyNamedElementCollectionBase<CommandArgument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArgumentCollection"/> class
        /// </summary>
        /// <param name="arguments">arguments doc goes here</param>
        public CommandArgumentCollection(CommandArgument[] arguments) : base(arguments)
        {
        }
    }
}
