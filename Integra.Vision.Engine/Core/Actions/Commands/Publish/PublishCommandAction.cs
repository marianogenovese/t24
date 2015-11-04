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
    using System.Threading.Tasks.Dataflow;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Event;

    /// <summary>
    /// Implements all the process of publish an event.
    /// </summary>
    internal sealed class PublishCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Transaction counter
        /// </summary>
        private static long contador = 0;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                this.SendEventToSource(command as PublishCommand);

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
        /// <param name="publishCommand">Publish command with the of the source</param>
        private async void SendEventToSource(PublishCommand publishCommand)
        {
            try
            {
                await Sources.GetSource(publishCommand.SourceName).SendAsync(publishCommand.Event);
                byte[] result;
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, "ok");
                    result = ms.ToArray();
                }

                publishCommand.Callback.GetCallbackChannel<ICommandResultCallback>().ConfirmEventPublication(new CallbackResult(result));
            }
            catch (Exception e)
            {
                byte[] result = System.Text.Encoding.UTF8.GetBytes(e.ToString());

                publishCommand.Callback.GetCallbackChannel<ICommandResultCallback>().ConfirmEventPublication(new CallbackResult(result));
            }
        }
    }
}
