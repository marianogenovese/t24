//-----------------------------------------------------------------------
// <copyright file="DataSet.cs" company="Integra.Vision.Engine  ">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Represents a set of rows.
    /// </summary>
    internal sealed class DataSet
    {
        /// <summary>
        /// The columns of the data set.
        /// </summary>
        private readonly DataColumnCollection columns;

        /// <summary>
        /// The rows of the data set.
        /// </summary>
        private readonly DataRowCollection rows;

        /// <summary>
        /// The row validator.
        /// </summary>
        private readonly DataRowValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSet"/> class.
        /// </summary>
        /// <param name="columns">The columns which the data sets contains.</param>
        public DataSet(DataColumn[] columns)
        {
            this.columns = new DataColumnCollection(this, columns);
            this.rows = new DataRowCollection(this, this.validator);
            this.validator = new DataRowValidator(this);
        }

        /// <summary>
        /// Gets the collection of columns defined. 
        /// </summary>
        public DataColumnCollection Columns
        {
            get
            {
                return this.columns;
            }
        }

        /// <summary>
        /// Gets the collection of rows that belong to this data set. 
        /// </summary>
        public DataRowCollection Rows
        {
            get
            {
                return this.rows;
            }
        }
    }
}
