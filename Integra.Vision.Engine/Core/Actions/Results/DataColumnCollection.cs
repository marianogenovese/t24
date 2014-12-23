//-----------------------------------------------------------------------
// <copyright file="DataColumnCollection.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Linq;
    
    /// <summary>
    /// Represents the collection of columns for DataSet
    /// </summary>
    internal sealed class DataColumnCollection : IEnumerable<DataColumn>
    {
        /// <summary>
        /// The columns defined in the data set.
        /// </summary>
        private readonly InternalDataColumnCollection columns;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataColumnCollection"/> class.
        /// </summary>
        /// <param name="set">The data set that belong this collection.</param>
        /// <param name="columns">The columns defined in the data set.</param>
        public DataColumnCollection(DataSet set, DataColumn[] columns)
        {
            Contract.Requires(columns != null);
            Contract.Requires(columns.Length > 0);
            this.columns = new InternalDataColumnCollection();
            foreach (DataColumn column in columns)
            {
                column.Set = set;
                this.columns.Add(column);
            }
        }

        /// <summary>
        /// Gets the total number of <see cref="DataColumn"/> objects in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.columns.Count;
            }
        }

        /// <summary>
        /// Gets the <see cref="DataColumn"/> from the collection at the specified index.
        /// </summary>
        /// <param name="columnIndex">The zero-based index of the column.</param>
        /// <returns>The <see cref="DataColumn"/> at the specified index.</returns>
        public DataColumn this[int columnIndex]
        {
            get
            {
                return this.columns[columnIndex];
            }
        }

        /// <summary>
        /// Gets the <see cref="DataColumn"/> from the collection with the specified name.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The <see cref="DataColumn"/> in the collection with the specified <see cref="DataColumn.Name"/>; otherwise a null value if the <see cref="DataColumn"/> does not exist.</returns>
        public DataColumn this[string columnName]
        {
            get
            {
                return this.columns[columnName];
            }
        }
        
        /// <inheritdoc />
        public IEnumerator<DataColumn> GetEnumerator()
        {
            return this.columns.GetEnumerator() as IEnumerator<DataColumn>;
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.columns.GetEnumerator();
        }

        /// <summary>
        /// Get the index of the column.
        /// </summary>
        /// <param name="column">The column to locate</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire <see cref="DataColumnCollection"/>, if found; otherwise, -1.</returns>
        internal int GetColumnIndex(DataColumn column)
        {
            Contract.Requires(column != null);
            return this.columns.IndexOf(column);
        }
    }
}
