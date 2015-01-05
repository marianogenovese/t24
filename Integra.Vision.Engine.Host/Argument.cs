//-----------------------------------------------------------------------
// <copyright file="Argument.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Represents an argument of a list of passed arguments
    /// </summary>
    internal sealed class Argument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/> class
        /// </summary>
        /// <param name="name">Name associated with the argument</param>
        /// <param name="description">Description associated with the argument</param>
        /// <param name="action">Action associated with the argument</param>
        public Argument(string name, string description, Action<Arguments, string> action)
        {
            this.Names = name.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            this.Action = action;
            this.Description = description;
        }

        /// <summary>
        /// Gets the list of names associates with the argument
        /// </summary>
        public IEnumerable<string> Names
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the action associated with the argument
        /// </summary>
        public Action<Arguments, string> Action
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the description associated with the argument
        /// </summary>
        public string Description
        {
            get;
            private set;
        }
    }
}
