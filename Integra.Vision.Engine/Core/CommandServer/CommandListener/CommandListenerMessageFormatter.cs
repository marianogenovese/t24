//-----------------------------------------------------------------------
// <copyright file="CommandListenerMessageFormatter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Linq;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// Provides a message transformation from client requests message to a proper server request entity.
    /// Also provides the response transformation.
    /// </summary>
    internal sealed class CommandListenerMessageFormatter : IDispatchMessageFormatter
    {
        /// <summary>
        /// This method implements the read of the message received from the client as a request and transform it to a proper server request entity.
        /// </summary>
        /// <param name="message">The message received from the client.</param>
        /// <param name="parameters">Used for set the proper parameter to the service operation method.</param>
        public void DeserializeRequest(Message message, object[] parameters)
        {
            OperationRequest request = null;
            if (!message.TryConvertToOperationRequest(out request))
            {
                throw new InvalidOperationException();
            }

            // Se asigna al arreglo el indice 0 pues el metodo tiene un solo parametro de entrada
            // el requerimiento construido a partir del mensaje recibido desde el cliente.
            parameters[0] = request;
        }

        /// <summary>
        /// This method implements the transformation of the result of executing the request to a proper client message
        /// </summary>
        /// <param name="messageVersion">The result message version.</param>
        /// <param name="parameters">The parameters marked as output.</param>
        /// <param name="result">The result object.</param>
        /// <returns>A proper client response message.</returns>
        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            OperationResponse response = result as OperationResponse;
            return Message.CreateMessage(messageVersion, Resources.SR.ExecuteReplyAction, response.Results.ToArray());            
            
            // return new VisionClientResponseMessage(response.OutputStream);
        }
    }
}
