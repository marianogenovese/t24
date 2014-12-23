//-----------------------------------------------------------------------
// <copyright file="OperationResponse.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contains information related to the response used for the proper execution of the operation. A operation response is a collection of results.
    /// </summary>
    internal sealed class OperationResponse
    {
        /// <summary>
        /// The results of the operation.
        /// </summary>
        private readonly CommandActionResultCollection results;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResponse"/> class.
        /// </summary>
        public OperationResponse()
        {
            this.results = new CommandActionResultCollection();
        }

        /// <summary>
        /// Gets the results of the operation.
        /// </summary>
        public CommandActionResultCollection Results
        {
            get
            {
                return this.results;
            }
        }
    }
}
