//-----------------------------------------------------------------------
// <copyright file="DiagnosticsEventIds.cs" company="Integra.Vision.Diagnostics">
//     Copyright (c) Integra.Vision.Diagnostics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Diagnostics
{
    /// <summary>
    /// Diagnostics event Ids.
    /// </summary>
    internal static class DiagnosticsEventIds
    {
        /// <summary>
        /// Id base for events.
        /// </summary>
        public const int Diagnostics = 0X01000;
        
        /// <summary>
        /// Unload AppDomain event.
        /// </summary>
        public const int AppDomainUnload = DiagnosticsEventIds.Diagnostics | 0X0001;
        
        /// <summary>
        /// Unobserved Task event.
        /// </summary>
        public const int UnobservedTask = DiagnosticsEventIds.Diagnostics | 0X0002;
    }
}
