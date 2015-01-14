//-----------------------------------------------------------------------
// <copyright file="IRepositoryForViews.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Repositories
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// IRepository interface
    /// </summary>
    /// <typeparam name="T">doc goes here</typeparam>
    internal interface IRepositoryForViews<T> : IDisposable where T : class
    {
        /// <summary>
        /// Gets the specify objects
        /// </summary>
        /// <param name="condition">where condition</param>
        /// <param name="projection">projection statement</param>
        /// <param name="conditionParameters">where condition parameters</param>
        /// <returns>list of objects</returns>
        IEnumerable Query(string condition, string projection, params object[] conditionParameters);
    }
}
