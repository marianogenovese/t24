//-----------------------------------------------------------------------
// <copyright file="EngineHost.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host
{
    using System;
    using System.Configuration;
    using Integra.Vision.Diagnostics;
    using Integra.Vision.Engine.Host.Configuration;
    using Integra.Vision.Engine.Host.Runtime;

    /// <summary>
    /// Implements a Windows Service Host for Integra Vision Engine
    /// </summary>    
    internal sealed class EngineHost : ServiceHostBase
    {
        /// <summary>
        /// Context execution arguments.
        /// </summary>
        private string[] args;

        /// <summary>
        /// The Integra Runtime.
        /// </summary>
        private RuntimeEngine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineHost"/> class.
        /// </summary>
        /// <param name="args">Context execution arguments.</param>
        public EngineHost(string[] args)
        {
            this.args = args;
            this.EventLog.Source = this.Name;
        }

        /// <summary>
        /// Gets the service name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Resources.SR.ServiceName;
            }
        }

        /// <summary>
        /// Gets the service description.
        /// </summary>
        public override string Description
        {
            get
            {
                return Resources.SR.ServiceDescription;
            }
        }
        
        /// <summary>
        /// Invoked during the service start.
        /// </summary>
        /// <param name="args">Start arguments.</param>
        protected override void OnStart(string[] args)
        {
            string basePath = null;
            
            try
            {
                // Parseo de agumentos                
                var argumentSet = new Arguments()
                    {
                        { "c|console", Resources.SR.ConsoleArgDescription, (p, value) => { } },
                        { "p|path", Resources.SR.EntryPointPathArgDescription, (p, value) => { basePath = value; } }
                    };

                if (!argumentSet.Parse(this.args))
                {
                    throw new ArgumentException(Resources.SR.InvalidArguments);
                }

                if (string.IsNullOrEmpty(basePath))
                {
                    throw new ArgumentException(Resources.SR.InvalidPathArgument, "path");
                }
                
                if (!ArgumentOperations.IsValidDirectory(basePath))
                {
                    throw new ArgumentException(Resources.SR.InvalidPathArgument, "path");
                }

                lock (this)
                {
                    this.engine = new RuntimeEngine(basePath);
                    this.engine.Start();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        /// <summary>
        /// Invoked during the service stop.
        /// </summary>
        protected override void OnStop()
        {
            lock (this)
            {
                if (this.engine != null)
                {
                    try
                    {
                        this.engine.Stop();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }
    }
}
