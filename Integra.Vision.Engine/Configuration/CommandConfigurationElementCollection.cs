//-----------------------------------------------------------------------
// <copyright file="CommandConfigurationElementCollection.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Configuration
{
    using System.Configuration;
    using System.Xml;
    
    /// <summary>
    /// Implements the logic related to recognize a command element in the command configuration.
    /// </summary>
    internal partial class CommandConfigurationElementCollection
    {
        /*
         * Aqui se controlan los elementos de la configuración que no son reconocidos.
         */

        /// <summary>
        /// Create a new runtime command configuration element.
        /// </summary>
        /// <param name="elementName">This parameter is ignored.</param>
        /// <returns>A new runtime command configuration element.</returns>
        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new RuntimeCommandConfigurationElement();
        }

        /// <summary>
        /// Handle a command definition.
        /// </summary>
        /// <param name="elementName">The name of the command.</param>
        /// <param name="reader">Configuration reader.</param>
        /// <returns>True if the command definition has been handled.</returns>
        private bool HandleUnrecognizedElement(string elementName, XmlReader reader)
        {
            RuntimeCommandConfigurationElement element = (RuntimeCommandConfigurationElement)this.CreateNewElement(elementName);
            element.CommandType = elementName;
            element.DeserializeInternal(reader);
            this.BaseAdd(element);
            return true;
        }        
    }
}
