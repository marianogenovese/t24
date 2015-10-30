﻿//-----------------------------------------------------------------------
// <copyright file="GroupByKeyComparer.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Runtime
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Group key comparer class
    /// </summary>
    /// <typeparam name="T">Type to compare</typeparam>
    public class GroupByKeyComparer<T> : IEqualityComparer<T>
    {
        /// <inheritdoc />
        public bool Equals(T x, T y)
        {
            foreach (PropertyInfo p in x.GetType().GetProperties())
            {
                var a = p.GetValue(x);
                var b = y.GetType().GetProperty(p.Name).GetValue(y);

                bool areEqual;
                if (a == null)
                {
                    areEqual = a == b;
                }
                else
                {
                    areEqual = a.Equals(b);
                }

                if (!areEqual)
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public int GetHashCode(T obj)
        {
            return 0;
        }
    }
}