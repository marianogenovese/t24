//-----------------------------------------------------------------------
// <copyright file="Dependency.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    /// <summary>
    /// Dependency class
    /// </summary>
    internal sealed class Dependency
    {
        /// <summary>
        /// Gets or sets the dependency
        /// </summary>
        public UserDefinedObject DependencyObject { get; set; }

        /// <summary>
        /// Gets or sets the dependency id
        /// </summary>
        public System.Guid DependencyObjectId { get; set; }

        /// <summary>
        /// Gets or sets the user defined object
        /// </summary>
        public UserDefinedObject PrincipalObject { get; set; }

        /// <summary>
        /// Gets or sets the user defined object id
        /// </summary>
        public System.Guid PrincipalObjectId { get; set; }
    }
}
