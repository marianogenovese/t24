//-----------------------------------------------------------------------
// <copyright file="DiagnosticsEventIds.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Diagnostics
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
        /// Application configuration file not found.
        /// </summary>
        public const int AppConfigFileNotFound = DiagnosticsEventIds.Diagnostics | 0X0001;
        
        /// <summary>
        /// Runtime started.
        /// </summary>
        public const int RuntimeStarted = DiagnosticsEventIds.Diagnostics | 0X0002;

        /// <summary>
        /// Dependency file enumeration.
        /// </summary>
        public const int FileEnumeration = DiagnosticsEventIds.Diagnostics | 0X0003;

        /// <summary>
        /// Dependency file loading.
        /// </summary>
        public const int DependencyAssemblyLoading = DiagnosticsEventIds.Diagnostics | 0X0004;

        /// <summary>
        /// Dependency instance creation.
        /// </summary>
        public const int DependencyInstanceCreation = DiagnosticsEventIds.Diagnostics | 0X0005;

        /// <summary>
        /// Object Wrapper invocation
        /// </summary>
        public const int RuntimeObjectWrapperInvocation = DiagnosticsEventIds.Diagnostics | 0X0006;

        /// <summary>
        /// Engine Module Build Event.
        /// </summary>
        public const int ModuleBuildEvent = DiagnosticsEventIds.Diagnostics | 0X0007;

        /// <summary>
        /// Engine Module Start Event.
        /// </summary>
        public const int ModuleStartEvent = DiagnosticsEventIds.Diagnostics | 0X0008;

        /// <summary>
        /// Engine Module Stop Event.
        /// </summary>
        public const int ModuleStopEvent = DiagnosticsEventIds.Diagnostics | 0X0009;

        /// <summary>
        /// Engine Module Abort Event.
        /// </summary>
        public const int ModuleAbortEvent = DiagnosticsEventIds.Diagnostics | 0X0010;

        /// <summary>
        /// Configuration file.
        /// </summary>
        public const int ConfigurationFileEvent = DiagnosticsEventIds.Diagnostics | 0X0011;
    }
}
