//-----------------------------------------------------------------------
// <copyright file="QueryCommandResult.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Represents a query result of a command action.
    /// </summary>
    [DataContract(Name = "QueryCommandResult", Namespace = "http://Integra.Vision.Engine/")]
    internal sealed class QueryCommandResult : CommandActionResult
    {
    }
}
