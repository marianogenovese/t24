//-----------------------------------------------------------------------
// <copyright file="CommandListenerCertificate.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Provides the certificate used in transport for secure communication.
    /// </summary>
    internal sealed class CommandListenerCertificate : X509Certificate2
    {
        /// <summary>
        /// Used for default certificate used in the command listener.
        /// </summary>
        private static CommandListenerCertificate current;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandListenerCertificate"/> class.
        /// </summary>
        public CommandListenerCertificate() : base(CommandListenerCertificate.CertificateData)
        {
        }

        /// <summary>
        /// Gets the current configured service certificate.
        /// </summary>
        public static CommandListenerCertificate Current
        {
            get
            {
                if (current == null)
                {
                    current = new CommandListenerCertificate();
                }

                return current;
            }
        }

        /// <summary>
        /// Gets the data for the certificate.
        /// </summary>
        private static byte[] CertificateData
        {
            get
            {
                return Convert.FromBase64String(Resources.SR.CertificateData);
            }
        }
    }
}
