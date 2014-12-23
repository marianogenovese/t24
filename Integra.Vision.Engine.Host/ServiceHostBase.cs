//-----------------------------------------------------------------------
// <copyright file="ServiceHostBase.cs" company="CompanyName">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Provides a base class for hosting Windows Services than can be installed automatically or run in console mode
    /// </summary>
    internal abstract class ServiceHostBase : ServiceBase
    {
        /// <summary>
        /// Flag used to check if is in console mode.
        /// </summary>
        private bool consoleMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostBase"/> class.
        /// </summary>
        public ServiceHostBase()
        {
            this.Initialize();
        }

        /// <summary>
        /// Gets the service name.
        /// </summary>
        public abstract string Name
        {
            get;
        }
        
        /// <summary>
        /// Gets the service description.
        /// </summary>
        public abstract string Description
        {
            get;
        }
        
        /// <summary>
        /// Internal start routine for console mode.
        /// </summary>
        internal void Start()
        {
            this.consoleMode = true;
            this.OnStart(null);
        }
        
        /// <summary>
        /// Implements free resource logic.
        /// </summary>
        /// <param name="disposing">Is disposing flag.</param>
        protected override void Dispose(bool disposing)
        {
            this.OnStop();
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Attach to event for handle unhandled exceptions.
        /// </summary>
        private void EnableNonTransientErrorsHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.OnAppDomainUnhandledException);
            TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(this.OnUnobservedTaskException);
        }
        
        /// <summary>
        /// Service initialization.
        /// </summary>
        private void Initialize()
        {
            this.CanPauseAndContinue = false;   
            
            // this.AutoLog = false;
            // Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }

        /// <summary>
        /// Handler of unobserved task exceptions.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The unobserved task exception event args.</param>
        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            this.NotifyError(e.Exception);
        }

        /// <summary>
        /// Handler of unhandled exceptions.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The unobserved task exception event args.</param>
        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.NotifyError((Exception)e.ExceptionObject);
        }

        /// <summary>
        /// Error notification handler.
        /// </summary>
        /// <param name="error">The error occurred.</param>
        private void NotifyError(Exception error)
        {
            if (this.consoleMode)
            {
                Console.WriteLine(error.ToString());
            }
            else
            {
                EventLog.WriteEntry(error.ToString(), EventLogEntryType.Error);
            }
        }
    }
}
