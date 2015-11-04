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
    [ServiceContract(Namespace = @"http://Integra.Vision.Engine/", Name = "CommandServer", SessionMode = SessionMode.Required, CallbackContract = typeof(ICommandResultCallback))]
    internal interface ICommandRequestHandler
    {
        /// <summary>
        /// This method allow to implement application logic to handle client command requests.
        /// </summary>
        /// <param name="request">The request information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [OperationContract(Action = "Execute", ReplyAction = "ExecuteReply", AsyncPattern = true)]
        Task<OperationResponse> Handle(OperationRequest request);

        /// <summary>
        /// Test method
        /// </summary>
        /// <param name="request">Operation request</param>
        [OperationContract(Action = "Receive", IsOneWay = true)]
        void Receive(OperationRequest request);

        /// <summary>
        /// Test method
        /// </summary>
        /// <param name="request">Operation request</param>
        [OperationContract(Action = "Publish", IsOneWay = true)]
        void Publish(OperationRequest request);
    }

    /// <summary>
    /// Callback test interface
    /// </summary>
    internal interface ICommandResultCallback
    {
        /// <summary>
        /// GetProjection callback
        /// </summary>
        /// <param name="result">CallbackResult to return</param>
        [OperationContract(IsOneWay = true)]
        void GetProjection(CallbackResult result);

        /// <summary>
        /// Callback test method
        /// </summary>
        /// <param name="result">OkCommandResult to return</param>
        [OperationContract(IsOneWay = true)]
        void ConfirmEventPublication(CallbackResult result);
    }
}
