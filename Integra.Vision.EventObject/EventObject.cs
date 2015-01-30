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
                        new MessageSection(0, "Header")
                        {
                            new MessageSubsection(0, "MessageType") { Value = "0100" }
                        },
                        new MessageSection(1, "Body")
                        {
                            new MessageSubsection(2, "PrimaryAccountNumber") { Value = "9999941616073663" },
                            new MessageSubsection(3, "ProcessingCode") { Value = "302000" },
                            new MessageSubsection(4, "TransactionAmount") { Value = 0m },
                            new MessageSubsection(7, "DateTimeTransmission") { Value = "0508152549" },
                            new MessageSubsection(11, "SystemTraceAuditNumber") { Value = "212868" },
                            new MessageSubsection(12, "LocalTransactionTime") { Value = "152549" },
                            new MessageSubsection(13, "LocalTransactionDate") { Value = "0508" },
                            new MessageSubsection(15, "SettlementDate") { Value = "0508" },
                            new MessageSubsection(18, "MerchantType") { Value = "6011" },
                            new MessageSubsection(19, "AcquiringInstitutionCountryCode") { Value = "320" },
                            new MessageSubsection(22, "PointOfServiceEntryMode") { Value = "051" },
                            new MessageSubsection(25, "PointOfServiceConditionCode") { Value = "02" },
                            new MessageSubsection(32, "AcquiringInstitutionIdentificationCode") { Value = "491381" },
                            new MessageSubsection(35, "Track2Data") { Value = "9999941616073663D18022011583036900000" },
                            new MessageSubsection(37, "RetrievalReferenceNumber") { Value = "412815212868" },
                            new MessageSubsection(41, "CardAcceptorTerminalIdentification") { Value = "2906    " },
                            new MessageSubsection(42, "CardAcceptorIdentificationCode") { Value = "Shell El Rodeo " },
                            new MessageSubsection(43, "CardAcceptorNameLocation") { Value = "Shell El RodeoGUATEMALA    GT" },
                            new MessageSubsection(49, "TransactionCurrencyCode") { Value = "320" },
                            new MessageSubsection(102, "AccountIdentification1") { Value = "00001613000000000001" },
                            new MessageSubsection(103, "Campo103")
                            { 
                                new MessageSubsection(1, "Campo103.1") { Value = "Dato del 103.1" }
                            },
                            new MessageSubsection(104, "campo104") { Value = -1 },
                            new MessageSubsection(105, "campo105") { Value = 1 }
                        }
                    };
                }

                return this.message;
            }
        }
    }
}
