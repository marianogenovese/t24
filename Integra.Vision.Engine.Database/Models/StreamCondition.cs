//-----------------------------------------------------------------------
// <copyright file="StreamCondition.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    /// <summary>
    /// StreamCondition class
    /// </summary>
    internal sealed class StreamCondition
    {
        /// <summary>
        /// Gets or sets the condition type
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the condition expression
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the associated stream
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets the stream id
        /// </summary>
        public System.Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is on condition
        /// </summary>
        public bool IsOnCondition { get; set; }
    }
}
