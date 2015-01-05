//-----------------------------------------------------------------------
// <copyright file="Assembly.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Assembly class
    /// </summary>
    internal sealed class Assembly : UserDefinedObject
    {
        /// <summary>
        /// Gets or sets the assembly local path
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// Gets or sets the adapter
        /// </summary>
        public ICollection<Adapter> Adapters { get; set; }
    }
}
