//-----------------------------------------------------------------------
// <copyright file="EventObject.cs" company="Ingetra.Vision.EventObject">
//     Copyright (c) Ingetra.Vision.EventObject. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Event
{
    using System;

    /// <summary>
    /// Event object class
    /// </summary>
    [Serializable]
    public class EventObject
    {
        /// <summary>
        /// Agent 
        /// Doc go here
        /// </summary>
        private EventAgent agent;

        /// <summary>
        /// adapter
        /// Doc go here
        /// </summary>
        private EventAdapter adapter;

        /// <summary>
        /// message
        /// Doc go here
        /// </summary>
        private EventMessage message;

        /// <summary>
        /// Gets or sets the agent
        /// </summary>
        public EventAgent Agent
        {
            get
            {
                if (this.agent == null)
                {
                    this.agent = new EventAgent();
                }

                return this.agent;
            }

            set
            {
                this.agent = value;
            }
        }

        /// <summary>
        /// Gets or sets the adapter
        /// </summary>
        public EventAdapter Adapter
        {
            get
            {
                if (this.adapter == null)
                {
                    this.adapter = new EventAdapter();
                }

                return this.adapter;
            }

            set
            {
                this.adapter = value;
            }
        }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public EventMessage Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
            }
        }
    }
}
