//-----------------------------------------------------------------------
// <copyright file="OkCommandResult.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a result when an operation is completed.
    /// </summary>
    [DataContract(Name = "OkCommandResult", Namespace = "http://Integra.Vision.Engine/")]
    internal class OkCommandResult : CommandActionResult
    {
    }
}
