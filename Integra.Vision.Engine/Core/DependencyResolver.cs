//-----------------------------------------------------------------------
// <copyright file="DependencyResolver.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using Integra.Vision.Dependency;
    
    /// <summary>
    /// Allow to access to the system dependency resolver.
    /// </summary>
    internal static class DependencyResolver
    {
        /// <summary>
        /// Gets or sets the default system dependency resolver.
        /// </summary>
        public static IDependencyResolver Default { get; set; }
    }
}
