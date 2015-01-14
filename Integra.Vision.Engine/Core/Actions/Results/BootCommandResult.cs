//-----------------------------------------------------------------------
// <copyright file="BootCommandResult.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Integra.Vision.Engine.Commands;

    /// <summary>
    /// Represents a result of boot command action.
    /// </summary>
    [DataContract(Name = "BootCommandResult", Namespace = "http://Integra.Vision.Engine/")]
    internal class BootCommandResult : CommandActionResult
    {
        /// <summary>
        /// The query result.
        /// </summary>
        private readonly IEnumerable<Tuple<string, ObjectTypeEnum>> result;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootCommandResult"/> class.
        /// </summary>
        /// <param name="result">The query result.</param>
        public BootCommandResult(IEnumerable<Tuple<string, ObjectTypeEnum>> result)
        {
            this.result = result;
        }

        /// <summary>
        /// Gets the array of scripts
        /// </summary>
        public Tuple<string, ObjectTypeEnum>[] Scripts
        {
            get
            {
                return this.result.ToArray();
            }
        }
    }
}
