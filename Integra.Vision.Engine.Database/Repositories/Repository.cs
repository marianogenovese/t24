//-----------------------------------------------------------------------
// <copyright file="Repository.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Generic repository
    /// </summary>
    /// <typeparam name="TObject">generic object</typeparam>
    internal class Repository<TObject> : IRepository<TObject> where TObject : class
    {
        /// <summary>
        /// Context variable
        /// </summary>
        private ObjectsContext context = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TObject}"/> class
        /// </summary>
        /// <param name="context">actual context</param>
        public Repository(ObjectsContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets an instance of type TObject Database Set given
        /// </summary>
        protected DbSet<TObject> TObjectDbSet
        {
            get
            {
                return this.context.Set<TObject>();
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
        /// Returns the first record that meets the search expression passed as parameter
        /// </summary>
        /// <param name="predicate">filtering expression</param>
        /// <returns>found object</returns>
        public virtual TObject Find(System.Linq.Expressions.Expression<Func<TObject, bool>> predicate)
        {
            return this.TObjectDbSet.FirstOrDefault<TObject>(predicate);
        }

        /// <summary>
        /// Returns the all records
        /// </summary>
        /// <returns>all objects</returns>
        public virtual System.Collections.Generic.IEnumerable<TObject> GetAll()
        {
            return this.TObjectDbSet.AsEnumerable();
        }

        /// <summary>
        /// Add last entity as a parameter to the context, saving changes
        /// </summary>
        /// <param name="t">object to add</param>
        /// <returns>object added</returns>
        public virtual TObject Create(TObject t)
        {
            var newEntry = this.TObjectDbSet.Add(t);
            return newEntry;
        }

        /// <summary>
        /// Removes the context objects that satisfy the search condition as a parameter, saving changes
        /// </summary>
        /// <param name="predicate">search expression</param>
        public virtual void Delete(System.Linq.Expressions.Expression<Func<TObject, bool>> predicate)
        {
            if (this.Exists(predicate))
            {
                var objects = this.Filter(predicate);
                foreach (TObject actualObject in objects)
                {
                    this.TObjectDbSet.Remove(actualObject);
                }
            }
        }

        /// <summary>
        /// Updates the object passed as parameter values ​​in the context saving changes
        /// </summary>
        /// <param name="t">object to update</param>
        public void Update(TObject t)
        {
            if (t != null)
            {
                var entry = this.context.Entry(t);
                this.TObjectDbSet.Attach(t);
                entry.State = EntityState.Modified;
            }
        }

        /// <summary>
        /// commit the changes
        /// </summary>
        /// <returns>the saved state</returns>
        public int Commit()
        {
            return this.context.SaveChanges();
        }

        /// <summary>
        /// Returns a record set filtered by the expression given in parameter
        /// </summary>
        /// <param name="predicate">filter expression</param>
        /// <returns>set of filtered TObjects</returns>
        public IQueryable Filter(Expression<Func<TObject, bool>> predicate)
        {
            return this.TObjectDbSet.Where<TObject>(predicate).AsQueryable();
        }

        /// <summary>
        /// Returns whether the object exists or not
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>True if the object exists, otherwise false</returns>
        public bool Exists(Expression<Func<TObject, bool>> predicate)
        {
            return this.TObjectDbSet.Any<TObject>(predicate);
        }
    }
}
