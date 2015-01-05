//-----------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// IRepository interface
    /// </summary>
    /// <typeparam name="T">doc goes here</typeparam>
    internal interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Finds a object by the expression
        /// </summary>
        /// <param name="predicate">filter expression</param>
        /// <returns>the object found</returns>
        T Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Create a new database object.
        /// </summary>
        /// <param name="t">new object to create</param>
        /// <returns>object created</returns>
        T Create(T t);

        /// <summary>
        /// Delete database objects 
        /// </summary>
        /// <param name="predicate">filter expression</param>
        void Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Update a database object
        /// </summary>
        /// <param name="t">Object to update</param>
        void Update(T t);

        /// <summary>
        /// commit the changes
        /// </summary>
        /// <returns>the saved state</returns>
        int Commit();

        /// <summary>
        /// Returns the all records
        /// </summary>
        /// <returns>all objects</returns>
        System.Collections.Generic.IEnumerable<T> GetAll();

        /// <summary>
        /// Returns whether the object exists or not
        /// </summary>
        /// <param name="predicate">Object name to verify</param>
        /// <returns>True if the object exists, otherwise false</returns>
        bool Exists(Expression<Func<T, bool>> predicate);
    }
}
