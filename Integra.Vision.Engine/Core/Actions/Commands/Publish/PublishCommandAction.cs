//-----------------------------------------------------------------------
// <copyright file="PublishCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.IO;
    using System.Messaging;
    using System.Runtime.Serialization.Formatters.Binary;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Event;

    /// <summary>
    /// Implements all the process of publish an event.
    /// </summary>
    internal sealed class PublishCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    this.SendEventToSource(context, command as PublishCommand);
                }

                return new OkCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains the logic for send events to a specific source.
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="publishCommand">Publish command with the of the source</param>
        private void SendEventToSource(ObjectsContext context, PublishCommand publishCommand)
        {
            EventObject @event = publishCommand.Event;

            if (publishCommand.SourceName.Equals(SR.SourceHttpType, StringComparison.InvariantCultureIgnoreCase))
            {
                using (System.Messaging.MessageQueue colaHttp = new System.Messaging.MessageQueue(@".\Private$\HttpSource"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bf.Serialize(ms, @event);
                        Message m = new Message(ms.ToArray(), new BinaryMessageFormatter());
                        colaHttp.Send(m, "byte");
                    }
                }
            }
        }
    }
}
