//-----------------------------------------------------------------------
// <copyright file="SystemDependency.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemDependency class
    /// </summary>
    internal sealed class SystemDependency
    {
        /// <summary>
        /// Gets or sets the dependency id
        /// </summary>
        public System.Guid DependenceId { get; set; }

        /// <summary>
        /// Gets or sets the user defined object id
        /// </summary>
        public System.Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets the creation date of the dependency
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
