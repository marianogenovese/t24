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
        /// Used for invalid user validation.
        /// </summary>
        private readonly Claim invalidUserClaim = new Claim(Resources.SR.CommandServerServiceContractNameSpace + "InvalidUsernameClaim", true, Rights.PossessProperty);

        /// <summary>
        /// This method can be used for implement custom authorization.
        /// </summary>
        /// <param name="operationContext">The operational context.</param>
        /// <returns>true if access is granted; otherwise; otherwise false. The default is true.</returns>
        public override bool CheckAccess(System.ServiceModel.OperationContext operationContext)
        {
            // Se verifica si se tiene un claim que contenga el tipo usuario inválido, esta validación hace que 
            // al cliente le aparezca una excepción como usuario/password incorrecto
            ReadOnlyCollection<ClaimSet> claimsets = operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets;
            
            // Si no tiene ningun permiso, retorna falso.
            if (claimsets == null || claimsets.Count == 0)
            {
                return false;
            }
            
            foreach (ClaimSet claimSet in claimsets)
            {                
                // Si no tiene el permiso para conectarse, retorna false para indicar que no esta permitido
                if (claimSet.ContainsClaim(CommandListenerClaims.ConnectionAllowedClaim))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
