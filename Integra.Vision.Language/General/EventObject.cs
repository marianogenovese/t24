//-----------------------------------------------------------------------
// <copyright file="EventObject.cs" company="Integra.Vision.Language">
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
    using Integra.Messaging;

    /// <summary>
    /// EventObject class
    /// </summary>
    internal sealed class EventObject
    {
        /// <summary>
        /// agent 
        /// Doc go here
        /// </summary>
        private Agent agent;

        /// <summary>
        /// adapter
        /// Doc go here
        /// </summary>
        private Adapter adapter;

        /// <summary>
        /// message
        /// Doc go here
        /// </summary>
        private Integra.Messaging.Message message;

        /// <summary>
        /// Gets the agent
        /// </summary>
        public Agent Agent
        {
            get
            {
                if (this.agent == null)
                {
                    this.agent = new Agent();
                }

                return this.agent;
            }
        }

        /// <summary>
        /// Gets the adapter
        /// </summary>
        public Adapter Adapter
        {
            get
            {
                if (this.adapter == null)
                {
                    this.adapter = new Adapter();
                }

                return this.adapter;
            }
        }

        /// <summary>
        /// Gets the message
        /// </summary>
        public Integra.Messaging.Message Message
        {
            get
            {
                if (this.message == null)
                {
                    this.message = new Integra.Messaging.Message()
                    {
                        new MessagePart(0, "Header")
                        {
                            new MessageField(0, "MessageType") { Value = "0100" }
                        },
                        new MessagePart(1, "Body")
                        {
                            new MessageField(2, "PrimaryAccountNumber") { Value = "9999941616073663" },
                            new MessageField(3, "ProcessingCode") { Value = "302000" },
                            new MessageField(4, "TransactionAmount") { Value = 0m },
                            new MessageField(7, "DateTimeTransmission") { Value = "0508152549" },
                            new MessageField(11, "SystemTraceAuditNumber") { Value = "212868" },
                            new MessageField(12, "LocalTransactionTime") { Value = "152549" },
                            new MessageField(13, "LocalTransactionDate") { Value = "0508" },
                            new MessageField(15, "SettlementDate") { Value = "0508" },
                            new MessageField(18, "MerchantType") { Value = "6011" },
                            new MessageField(19, "AcquiringInstitutionCountryCode") { Value = "320" },
                            new MessageField(22, "PointOfServiceEntryMode") { Value = "051" },
                            new MessageField(25, "PointOfServiceConditionCode") { Value = "02" },
                            new MessageField(32, "AcquiringInstitutionIdentificationCode") { Value = "491381" },
                            new MessageField(35, "Track2Data") { Value = "9999941616073663D18022011583036900000" },
                            new MessageField(37, "RetrievalReferenceNumber") { Value = "412815212868" },
                            new MessageField(41, "CardAcceptorTerminalIdentification") { Value = "2906    " },
                            new MessageField(42, "CardAcceptorIdentificationCode") { Value = "Shell El Rodeo " },
                            new MessageField(43, "CardAcceptorNameLocation") { Value = "Shell El RodeoGUATEMALA    GT" },
                            new MessageField(49, "TransactionCurrencyCode") { Value = "320" },
                            new MessageField(102, "AccountIdentification1") { Value = "00001613000000000001" },
                            new MessageField(103, "Campo103")
                            { 
                                new MessageField(1, "Campo103.1") { Value = "Dato del 103.1" }
                            },
                            new MessageField(104, "campo104") { Value = -1 },
                            new MessageField(105, "campo105") { Value = 1 }
                        }
                    };
                }

                return this.message;
            }
        }
    }
}
