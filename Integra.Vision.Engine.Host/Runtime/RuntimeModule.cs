//-----------------------------------------------------------------------
// <copyright file="RuntimeModule.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Runtime
{
    using System.Diagnostics.CodeAnalysis;
    
    /// <summary>
    /// Defines the contract for the engine module.
    /// </summary>
    internal sealed class RuntimeModule : RuntimeObjectWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeModule"/> class.
        /// </summary>
        /// <param name="instance">The engine module instance.</param>
        public RuntimeModule(object instance) : base(instance)
        {
        }

        /// <summary>
        /// Causes a module to transition immediately from its current state into the stopped state.
        /// </summary>
        public void Abort()
        {
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleAbortEvent, string.Format("Trying to abort the module '{0}'...", this.Instance.GetType().FullName));
            if (!this.TryInvokeAction("Abort"))
            {
                throw new RuntimeException(Resources.SR.UnableToAbortModule(this.Instance.GetType().FullName));
            }
            
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleAbortEvent, string.Format("'{0}' Module has been aborted.", this.Instance.GetType().FullName));
        }

        /// <summary>
        /// Causes a module to transition from its current state into the stopped state.
        /// </summary>
        public void Stop()
        {
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleStopEvent, string.Format("Trying to stop the module '{0}'...", this.Instance.GetType().FullName));
            if (!this.TryInvokeAction("Stop"))
            {
                throw new RuntimeException(Resources.SR.UnableToStopModule(this.Instance.GetType().FullName));
            }

            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleStopEvent, string.Format("'{0}' Module has been stopped.", this.Instance.GetType().FullName));
        }

        /// <summary>
        /// Causes a module to transition from the created state into the started state.
        /// </summary>
        public void Start()
        {
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleStartEvent, string.Format("Trying to start the module '{0}'...", this.Instance.GetType().FullName));
            if (!this.TryInvokeAction("Start"))
            {
                throw new RuntimeException(Resources.SR.UnableToStartModule(this.Instance.GetType().FullName));
            }

            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.ModuleStartEvent, string.Format("'{0}' Module has been successfully started.", this.Instance.GetType().FullName));
        }
    }
}
