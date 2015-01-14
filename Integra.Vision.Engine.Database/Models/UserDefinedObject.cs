//-----------------------------------------------------------------------
// <copyright file="UserDefinedObject.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// UserDefinedObject class
    /// </summary>
    internal abstract class UserDefinedObject : Integra.Vision.Engine.Database.Models.SystemObject
    {
        /// <summary>
        /// Gets or sets the object name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the adapter state
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is a system object
        /// </summary>
        public bool IsSystemObject { get; set; }

        /// <summary>
        /// Gets or sets the dependencies
        /// </summary>
        public ICollection<Dependency> Dependencies { get; set; }

        /// <summary>
        /// Gets or sets the objects that have dependencies
        /// </summary>
        public ICollection<Dependency> PrincipalObjects { get; set; }

        /// <summary>
        /// Gets or sets the trace level
        /// </summary>
        public virtual SetTrace SetTrace { get; set; }

        /// <summary>
        /// Gets or sets the scripts
        /// </summary>
        public ICollection<Script> Scripts { get; set; }
    }
}
