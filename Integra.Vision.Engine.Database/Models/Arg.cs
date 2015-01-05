//-----------------------------------------------------------------------
// <copyright file="Arg.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    /// <summary>
    /// Argument class
    /// </summary>
    internal sealed class Arg
    {
        /// <summary>
        /// Gets or sets the data type of the argument
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the parameter name of the argument
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the argument
        /// </summary>
        public byte[] Value { get; set; }

        /// <summary>
        /// Gets or sets the adapter id
        /// </summary>
        public System.Guid AdapterId { get; set; }

        /// <summary>
        /// Gets or sets the adapter
        /// </summary>
        public Adapter Adapter { get; set; }
    }
}
