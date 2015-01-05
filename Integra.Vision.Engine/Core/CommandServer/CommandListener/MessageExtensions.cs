//-----------------------------------------------------------------------
// <copyright file="MessageExtensions.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.ServiceModel.Channels;

    /// <summary>
    /// Provides WCF message helper extensions.
    /// </summary>
    internal static class MessageExtensions
    {
        /// <summary>
        /// Try to convert the message into a proper command script request
        /// </summary>
        /// <param name="message">The message to be converted.</param>
        /// <param name="request">The request object as result of the conversion.</param>
        /// <returns>True if the message can be converted to a proper command script request.</returns>
        public static bool TryConvertToOperationRequest(this Message message, out OperationRequest request)
        {
            request = new OperationRequest(message.GetCommandText(), message.GetClientAddress());
            return true;
        }

        /// <summary>
        /// Get remote client address.
        /// </summary>
        /// <param name="message">The request message.</param>
        /// <returns>The address of the remote client.</returns>
        public static string GetClientAddress(this Message message)
        {
            return (message.Properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty).Address;
        }

        /// <summary>
        /// Get the command text of the client message request.
        /// </summary>
        /// <param name="message">The message request.</param>
        /// <returns>The command text.</returns>
        public static string GetCommandText(this Message message)
        {
            return message.GetReaderAtBodyContents().ReadContentAsString();
        }
    }
}
