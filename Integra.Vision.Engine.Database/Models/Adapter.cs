//-----------------------------------------------------------------------
// <copyright file="Adapter.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Adapter class
    /// </summary>
    internal sealed class Adapter : UserDefinedObject
    {
        /// <summary>
        /// Gets or sets the adapter type
        /// </summary>
        public int AdapterType { get; set; }

        /// <summary>
        /// Gets or sets the assembly
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets the adapter assembly reference
        /// </summary>
        public System.Guid AssemblyId { get; set; }

        /// <summary>
        /// Gets or sets the arguments of the adapter
        /// </summary>
        public ICollection<Arg> Args { get; set; }

        /// <summary>
        /// Gets or sets the source
        /// </summary>
        public ICollection<Source> Sources { get; set; }

        /// <summary>
        /// Gets or sets the Statement
        /// </summary>
        public ICollection<Stmt> Stmts { get; set; }
    }
}
