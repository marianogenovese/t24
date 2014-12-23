//-----------------------------------------------------------------------
// <copyright file="OperationRequest.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Contains information related to the request used for the proper execution of the operation.
    /// </summary>
    internal sealed class OperationRequest
    {
        /// <summary>
        /// The text of the script.
        /// </summary>
        private readonly string script;

        /// <summary>
        /// The address of the remote client.
        /// </summary>
        private readonly string clientAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationRequest"/> class.
        /// </summary>
        /// <param name="script">The text of the script.</param>
        /// <param name="clientAddress">The address of the remote client.</param>
        public OperationRequest(string script, string clientAddress)
        {
            this.script = script;
            this.clientAddress = clientAddress;
        }

        /// <summary>
        /// Gets the script of the request.
        /// </summary>
        public string Script
        {
            get
            {
                return this.script;
            }
        }

        /// <summary>
        /// Gets the address of the remote client.
        /// </summary>
        public string ClientAddress
        {
            get
            {
                return this.clientAddress;
            }
        }
    }
}
