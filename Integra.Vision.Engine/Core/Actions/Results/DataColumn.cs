//-----------------------------------------------------------------------
// <copyright file="DataColumn.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Represents a column in data set.
    /// </summary>
    internal abstract class DataColumn
    {
        /// <summary>
        /// The name of the column.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataColumn"/> class.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        public DataColumn(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Gets the column type.
        /// </summary>
        public abstract DataType Type
        {
            get;
        }

        /// <summary>
        /// Gets the column name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets or sets the Data set that belong to this column.
        /// </summary>
        protected internal DataSet Set
        {
            get;
            set;
        }

        /// <summary>
        /// Based on the information of the column, check if the value is valid.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is valid; otherwise false.</returns>
        public abstract bool Validate(object value);
    }
}
