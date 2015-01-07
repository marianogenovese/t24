//-----------------------------------------------------------------------
// <copyright file="UnitOfWorkForObjects.cs" company="Ingetra.Vision.Database">
//     Copyright (c) Ingetra.Vision.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Repositories
{
    using System;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Class that allow to multiple repositories share a single database context.
    /// </summary>
    /// <typeparam name="TObject">Database objet type</typeparam>
    internal class UnitOfWorkForObjects<TObject> : IDisposable where TObject : class
    {
        /// <summary>
        /// Current context
        /// </summary>
        private ViewsContext context = new ViewsContext("EngineDatabase");

        /// <summary>
        /// Repository of objects.
        /// </summary>
        private Repository<TObject> objectRepository;

        /// <summary>
        /// Doc goes here
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkForObjects{TObject}"/> class
        /// </summary>
        public UnitOfWorkForObjects()
        {
            this.context = new ViewsContext("EngineDatabase");
        }

        /// <summary>
        /// Gets the database objects repository
        /// </summary>
        public Repository<TObject> ObjectRepository
        {
            get
            {
                if (this.objectRepository == null)
                {
                    this.objectRepository = new Repository<TObject>(this.context);
                }

                return this.objectRepository;
            }
        }

        /// <summary>
        /// Dispose the current context
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Save the changes
        /// </summary>
        public void Commit()
        {
            this.context.SaveChanges();
        }

        /// <summary>
        /// Dispose the current context
        /// </summary>
        /// <param name="disposing">Doc goes here</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
