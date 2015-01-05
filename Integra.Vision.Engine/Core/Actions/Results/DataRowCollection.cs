//-----------------------------------------------------------------------
// <copyright file="DataRowCollection.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    
    /// <summary>
    /// Represents the collection of columns for DataSet
    /// </summary>
    internal sealed class DataRowCollection
    {
        /// <summary>
        /// The data set that belong this collection.
        /// </summary>
        private readonly DataSet set;

        /// <summary>
        /// The row validator.
        /// </summary>
        private readonly DataRowValidator validator;

        /// <summary>
        /// The rows.
        /// </summary>
        private List<DataRow> rows;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRowCollection"/> class.
        /// </summary>
        /// <param name="set">The data set that belong this collection.</param>
        /// <param name="validator">The row validator.</param>
        public DataRowCollection(DataSet set, DataRowValidator validator)
        {
            Contract.Requires(set != null);
            Contract.Requires(validator != null);
            this.set = set;
            this.validator = validator;
            this.rows = new List<DataRow>();
        }

        /// <summary>
        /// Gets the total number of <see cref="DataRow"/> objects in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Creates a row using specified values.
        /// </summary>
        /// <param name="values">The array of values that are used to create the new row.</param>
        /// <returns>The <see cref="DataRow"/> to added.</returns>
        public DataRow Add(params object[] values)
        {
            this.rows.Add(new DataRow(this.set, this.validator));
            return this.rows[this.rows.Count - 1];
        }

        /// <summary>
        /// Creates a row using specified values.
        /// </summary>
        /// <returns>The <see cref="DataRow"/> to added.</returns>
        public DataRow Add()
        {
            this.rows.Add(new DataRow(this.set, this.validator));
            return this.rows[this.rows.Count - 1];
        }
    }
}
