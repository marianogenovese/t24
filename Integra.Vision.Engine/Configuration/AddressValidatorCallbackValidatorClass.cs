//-----------------------------------------------------------------------
// <copyright file="AddressValidatorCallbackValidatorClass.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    
    /// <summary>
    /// Implements a address validator using Uri class.
    /// </summary>
    public partial class AddressValidatorCallbackValidatorClass
    {
        /// <summary>
        /// Validate the address.
        /// </summary>
        /// <param name="value">The address.</param>
        public static void ValidateAddress(object value)
        {
            Contract.Requires<ConfigurationException>(value != null);
            string address = value.ToString();
            
            // Esto es porque el framework llama dos veces a metodo validar pero la primera lo envia con el default que en este caso es en blanco.
            if (string.IsNullOrEmpty(address))
            {
                return;
            }
            
            Uri addressUri;
            
            if (!Uri.TryCreate(address, UriKind.Absolute, out addressUri))
            {
                throw new ConfigurationException();
            }
        }
    }
}
