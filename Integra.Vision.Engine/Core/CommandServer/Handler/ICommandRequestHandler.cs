//-----------------------------------------------------------------------
// <copyright file="ICommandRequestHandler.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Allow to implements a handler for incoming client requests.
    /// </summary>
    [ServiceContract(Namespace = @"http://Integra.Vision.Engine/", Name = "CommandServer", SessionMode = SessionMode.Required)]
    internal interface ICommandRequestHandler
    {
        /// <summary>
        /// This method allow to implement application logic to handle client command requests.
        /// </summary>
        /// <param name="request">The request information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [OperationContract(Action = "*", ReplyAction = "*", AsyncPattern = true)]
        Task<OperationResponse> Handle(OperationRequest request);
    }
}
