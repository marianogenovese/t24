//-----------------------------------------------------------------------
// <copyright file="IParseFilterContext.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// This interface implements a context used for parsing process.
    /// </summary>
    internal interface IParseFilterContext
    {
        // Aqui se debe agregar una propiedad solo de lectura de los nodos.
        
        /// <summary>
        /// Gets or sets the execution plan nodes.
        /// </summary>
        IEnumerable<PlanNode> Nodes { get; set; }
    }
}
