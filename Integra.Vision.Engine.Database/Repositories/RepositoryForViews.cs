//-----------------------------------------------------------------------
// <copyright file="RepositoryForViews.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Repositories
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Repository for system views
    /// </summary>
    /// <typeparam name="TView">Generic view</typeparam>
    internal class RepositoryForViews<TView> : IRepositoryForViews<TView> where TView : class
    {
        /// <summary>
        /// Context variable
        /// </summary>
        private SystemViewsContext context = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryForViews{TView}"/> class
        /// </summary>
        /// <param name="context">actual context</param>
        public RepositoryForViews(SystemViewsContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets an instance of type TView Database Set given
        /// </summary>
        private DbSet<TView> Set
        {
            get
            {
                return this.context.Set<TView>();
            }
        }

        /// <summary>
        /// Releasing resources
        /// </summary>
        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        /// <summary>
        /// Gets the specify objects
        /// </summary>
        /// <param name="condition">where condition</param>
        /// <param name="projection">projection statement</param>
        /// <param name="conditionParameters">where condition parameters</param>
        /// <returns>list of objects</returns>
        public object[] Query(string condition, string projection, params object[] conditionParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
