//-----------------------------------------------------------------------
// <copyright file="CallbackResult.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a result callback.
    /// </summary>
    [DataContract(Name = "CallbackResult", Namespace = "http://Integra.Vision.Engine/")]
    internal class CallbackResult : CommandActionResult
    {
        /// <summary>
        /// Result of the callback
        /// </summary>
        private byte[] result;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackResult"/> class.
        /// </summary>
        public CallbackResult()
        {
            this.result = System.Text.Encoding.UTF8.GetBytes(new OkCommandResult().ToString());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackResult"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public CallbackResult(byte[] result)
        {
            this.result = result;
        }

        /// <summary>
        /// Gets or sets the result of the callback
        /// </summary>
        [DataMember(Name = "Result")]
        public byte[] Result
        {
            get
            {
                return this.result;
            }

            set
            {
                this.result = value;
            }
        }
    }
}
