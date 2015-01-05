//-----------------------------------------------------------------------
// <copyright file="CommandListenerAuthorizationManager.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.ObjectModel;
    using System.IdentityModel.Claims;
    using System.ServiceModel;
    
    /// <summary>
    /// This class allow to implements custom authorization logic.
    /// </summary>
    internal sealed class CommandListenerAuthorizationManager : ServiceAuthorizationManager
    {
        /// <summary>
        /// This method can be used for implement custom authorization.
        /// </summary>
        /// <param name="operationContext">The operational context.</param>
        /// <returns>true if access is granted; otherwise; otherwise false. The default is true.</returns>
        public override bool CheckAccess(System.ServiceModel.OperationContext operationContext)
        {
            // Se verifica si se tiene un claim que contenga el tipo usuario inválido, esta validación hace que 
            // al cliente le aparezca una excepción como usuario/password incorrecto
            string invalidUserClaim = Resources.SR.CommandServerServiceContractNameSpace + "InvalidUsernameClaim";
            ReadOnlyCollection<ClaimSet> claimsets = operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets;
            foreach (ClaimSet claimSet in claimsets)
            {
                if (claimSet.ContainsClaim(new Claim(invalidUserClaim, true, Rights.PossessProperty)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
