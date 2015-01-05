//-----------------------------------------------------------------------
// <copyright file="Agent.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Agent class
    /// </summary>
    internal sealed class Agent
    {
        /// <summary>
        /// timestamp 
        /// Doc go here
        /// </summary>
        private DateTime timestamp;

        /// <summary>
        /// name
        /// Doc go here
        /// </summary>
        private string name;

        /// <summary>
        /// machineName
        /// Doc go here
        /// </summary>
        private string machineName;

        /// <summary>
        /// Gets the name of the agent
        /// </summary>
        public string Name
        {
            get
            {
                if (this.name == null)
                {
                    this.name = "Anonimo";
                }

                return this.name;
            }
        }

        /// <summary>
        /// Gets 
        /// Doc go here
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                if (this.timestamp == null)
                {
                    this.timestamp = DateTime.Now;
                }

                return this.timestamp;
            }
        }

        /// <summary>
        /// Gets
        /// Doc go here
        /// </summary>
        public string MachineName
        {
            get
            {
                if (this.machineName == null)
                {
                    this.machineName = "Anonimo";
                }

                return this.machineName;
            }
        }
    }
}
