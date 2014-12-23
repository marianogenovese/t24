//-----------------------------------------------------------------------
// <copyright file="SourceCondition.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    /// <summary>
    /// StreamCondition class
    /// </summary>
    internal sealed class SourceCondition
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
        public Source Source { get; set; }

        /// <summary>
        /// Gets or sets the stream id
        /// </summary>
        public System.Guid SourceId { get; set; }
    }
}
