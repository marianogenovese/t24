//-----------------------------------------------------------------------
// <copyright file="CommandActionResultCollection.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Contains a collection of command action results.
    /// </summary>
    [CollectionDataContract(Name = "CommandActionResultCollection", Namespace = "http://Integra.Vision.Engine/", IsReference = true, ItemName = "CommandActionResult")]
    internal sealed class CommandActionResultCollection : Collection<CommandActionResult>
    {
    }
}
