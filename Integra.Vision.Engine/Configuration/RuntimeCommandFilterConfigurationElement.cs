//-----------------------------------------------------------------------
// <copyright file="RuntimeCommandFilterConfigurationElement.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Configuration
{
    using System.Xml;
    
    /// <summary>
    /// Implements a dynamic command filter definition.
    /// </summary>
    internal sealed class RuntimeCommandFilterConfigurationElement : FilterConfigurationElement
    {
        /// <summary>
        /// Deserialize the configuration element.
        /// </summary>
        /// <param name="reader">The configuration reader.</param>
        public void DeserializeInternal(XmlReader reader)
        {
            this.DeserializeElement(reader, false);
        }
    }
}
