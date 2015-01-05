//-----------------------------------------------------------------------
// <copyright file="EngineClaimTypes.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Represents the pre-defined types of claims that an entity can claim. 
    /// </summary>
    internal static class EngineClaimTypes
    {
        /// <summary>
        /// Id for connection allow claim.
        /// </summary>
        private static string connectionAllowedClaim = Resources.SR.CommandServerServiceContractNameSpace + "ConnectionAllowedClaim";
        
        /// <summary>
        /// Id for create assembly claim.
        /// </summary>
        private static string createAssemblyClaim = Resources.SR.CommandServerServiceContractNameSpace + "CreateAssemblyClaim";

        /// <summary>
        /// Gets the type for a claim that specifies a connection allowed for an entity.
        /// </summary>
        public static string ConnectionAllowed
        {
            get
            {
                return connectionAllowedClaim;
            }
        }

        /// <summary>
        /// Gets the type for a claim that specifies a create assembly claim.
        /// </summary>
        public static string CreateAssemblyClaim
        {
            get
            {
                return createAssemblyClaim;
            }
        }
    }
}
