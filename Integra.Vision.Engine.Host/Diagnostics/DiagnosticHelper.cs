//-----------------------------------------------------------------------
// <copyright file="DiagnosticHelper.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Diagnostics
{
    using Integra.Vision.Diagnostics;
    
    /// <summary>
    /// Helper class for diagnostics.
    /// </summary>
    internal static class DiagnosticHelper
    {
        /// <summary>
        /// Hosting logger.
        /// </summary>
        private static readonly ILogger HostLogger = new LegacyLogger(Diagnostics.Resources.SR.RuntimeTraceSourceName, null);

        /// <summary>
        /// Gets the default logger.
        /// </summary>
        public static ILogger Logger
        {
            get
            {
                return HostLogger;
            }
        }
    }
}
