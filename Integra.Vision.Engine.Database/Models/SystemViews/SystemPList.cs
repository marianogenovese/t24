//-----------------------------------------------------------------------
// <copyright file="SystemPList.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemPList class
    /// </summary>
    internal sealed class SystemPList
    {
        /// <summary>
        /// Gets or sets the object alias
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the expression string
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the object position
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the stream id
        /// </summary>
        public System.Guid Id { get; set; }
    }
}
