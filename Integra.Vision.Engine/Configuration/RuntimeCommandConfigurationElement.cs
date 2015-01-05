//-----------------------------------------------------------------------
// <copyright file="RuntimeCommandConfigurationElement.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Configuration
{
    using System.Configuration;
    using System.Xml;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Core;
    
    /// <summary>
    /// Implements a dynamic command definition.
    /// </summary>
    internal class RuntimeCommandConfigurationElement : CommandConfigurationElement
    {
        /// <summary>
        /// Gets the command type as enumerator.
        /// </summary>
        public CommandTypeEnum CommandTypeAsEnum
        {
            get
            {
                return this.CommandType.AsCommandTypeEnum();
            }
        }
        
        /// <summary>
        /// Deserialize the configuration element.
        /// </summary>
        /// <param name="reader">The configuration reader.</param>
        public void DeserializeInternal(XmlReader reader)
        {
            this.DeserializeElement(reader, true);
        }

        /// <summary>
        /// Creates a new filter configuration element.
        /// </summary>
        /// <param name="elementName">This parameter is ignored.</param>
        /// <returns>A new filter configuration element.</returns>
        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new RuntimeCommandFilterConfigurationElement();
        }

        /// <summary>
        /// Create a new filter configuration element.
        /// </summary>
        /// <param name="elementName">This parameter is ignored.</param>
        /// <param name="reader">Configuration reader.</param>
        /// <returns>True if the filter definition has been handled.</returns>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            RuntimeCommandFilterConfigurationElement element = (RuntimeCommandFilterConfigurationElement)this.CreateNewElement(elementName);
            element.DeserializeInternal(reader);
            this.BaseAdd(element);
            return true;
        }
    }
}
