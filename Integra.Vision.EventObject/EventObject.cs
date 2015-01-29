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
        /// Gets the agent
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
        }

        /// <summary>
        /// Gets the adapter
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
        }

        /// <summary>
        /// Gets the message
        /// </summary>
        public EventMessage Message
        {
            get
            {
                if (this.message == null)
                {
                    this.message = new EventMessage() 
                    { 
                        new MessageSection(1, "Section 1")
                        {
                            new MessageSubsection(1, "sub-1.1") 
                            {
                                new MessageSubsection(1, "sub-1.1.1") { Value = 1234 },
                                new MessageSubsection(2, "sub-1.1.2") { Value = "Cadena" }
                            }
                        },
                        new MessageSection(2, "Section 2")
                        {
                            new MessageSubsection(1, "sub-2.1") 
                            {
                                new MessageSubsection(1, "sub-2.1.1") { Value = true }
                            },
                            new MessageSubsection(2, "sub-2.2") 
                            {
                                new MessageSubsection(1, "sub-2.2.1") { Value = "asdf" }
                            }
                        } 
                    };
                }

                return this.message;
            }
        }
    }
}
