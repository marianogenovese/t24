//-----------------------------------------------------------------------
// <copyright file="DataRow.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Represents a row in a data set.
    /// </summary>
    internal sealed class DataRow
    {
        /// <summary>
        /// The row validator.
        /// </summary>
        private readonly DataSet set;

        /// <summary>
        /// The row validator.
        /// </summary>
        private readonly DataRowValidator validator;

        /// <summary>
        /// The values storage.
        /// </summary>
        private object[] storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRow"/> class.
        /// </summary>
        /// <param name="set">The data set that belong this row.</param>
        /// <param name="validator">The row validator.</param>
        protected internal DataRow(DataSet set, DataRowValidator validator)
        {
            Contract.Requires(set != null);
            Contract.Requires(validator != null);
            this.set = set;
            this.validator = validator;
            this.storage = new object[set.Columns.Count];
        }

        /// <summary>
        /// Gets or sets the data stored in the column specified by index.
        /// </summary>
        /// <param name="columnIndex">The zero-based index of the column.</param>
        /// <returns>An System.Object that contains the data.</returns>
        public object this[int columnIndex]
        {
            get
            {
                return this[this.set.Columns[columnIndex]];
            }
            
            set
            {
                this[this.set.Columns[columnIndex]] = value;
            }
        }

        /// <summary>
        /// Gets or sets the data stored in the column specified by name.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>An System.Object that contains the data.</returns>
        public object this[string columnName]
        {
            get
            {
                return this[this.set.Columns[columnName]];
            }
            
            set
            {
                this[this.set.Columns[columnName]] = value;
            }
        }

        /// <summary>
        /// Gets or sets the data stored in the column specified by name.
        /// </summary>
        /// <param name="column">The data column object that represent the column.</param>
        /// <returns>An System.Object that contains the data.</returns>
        public object this[DataColumn column]
        {
            get
            {
                int index = this.set.Columns.GetColumnIndex(column);
                return this.storage[this.set.Columns.GetColumnIndex(column)];
            }

            set
            {
                this.validator.Validate(column, value);
                this.storage[this.set.Columns.GetColumnIndex(column)] = value;
            }
        }
    }
}
