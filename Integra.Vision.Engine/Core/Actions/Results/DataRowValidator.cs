//-----------------------------------------------------------------------
// <copyright file="DataRowValidator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Diagnostics.Contracts;
    
    /// <summary>
    /// Implements the validation when a row is added
    /// </summary>
    internal sealed class DataRowValidator
    {
        /// <summary>
        /// The columns defined in the data set.
        /// </summary>
        private readonly DataSet set;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRowValidator"/> class.
        /// </summary>
        /// <param name="set">The columns defined in the data set.</param>
        public DataRowValidator(DataSet set)
        {
            Contract.Requires(set != null);
            this.set = set;
        }

        /// <summary>
        /// Verify if the row is valid.
        /// </summary>
        /// <param name="row">The row to validate.</param>
        public void Validate(DataRow row)
        {
            foreach (DataColumn column in this.set.Columns)
            {
                this.Validate(column, row[column]);
            }
        }

        /// <summary>
        /// Verify if the a value of a cell of the row is valid.
        /// </summary>
        /// <param name="column">The column of row that belong to the value.</param>
        /// <param name="value">The value to validate.</param>
        public void Validate(DataColumn column, object value)
        {
            try
            {
                if (!column.Validate(value))
                {
                    throw new DataSetException(Resources.SR.DataSetRowValueValidationError);
                }
            }
            catch (Exception e)
            {
                throw new DataSetException(Resources.SR.DataSetUnhandledException, e);
            }
        }
    }
}
