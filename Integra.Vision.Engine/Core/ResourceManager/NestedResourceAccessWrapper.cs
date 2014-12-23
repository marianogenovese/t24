//-----------------------------------------------------------------------
// <copyright file="NestedResourceAccessWrapper.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements an access that requires access to both the parent resource and the resource related to the request.
    /// </summary>
    /// <typeparam name="T">the type of resource that has been requested access.</typeparam>
    internal sealed class NestedResourceAccessWrapper<T> : ResourceAccess<T>
    {
        /// <summary>
        /// The access related to this request access.
        /// </summary>
        private readonly ResourceAccess currentResourceAccess;
        
        /// <summary>
        /// The parent access related to this request.
        /// </summary>
        private readonly ResourceAccess parentResourceAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="NestedResourceAccessWrapper{T}"/> class.
        /// </summary>
        /// <param name="currentResourceAccess">The access related to this request access.</param>
        /// <param name="parentResourceAccess">The parent access related to this request.</param>
        internal NestedResourceAccessWrapper(ResourceAccess<T> currentResourceAccess, ResourceAccess parentResourceAccess) : base(currentResourceAccess.Manager, currentResourceAccess.Resource, currentResourceAccess.IsExclusive)
        {
            this.currentResourceAccess = currentResourceAccess;
            this.parentResourceAccess = parentResourceAccess;
        }
        
        /// <inheritdoc />
        public override void Dispose()
        {
            this.parentResourceAccess.Dispose();
            this.currentResourceAccess.Dispose();
        }
        
        /// <inheritdoc />
        protected override void LockResource()
        {
            // Ojo ya no se hace el lock recurso porque ya se hizo cuando se creo el current resource access
        }
    }
}
