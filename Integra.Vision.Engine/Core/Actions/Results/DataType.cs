//-----------------------------------------------------------------------
// <copyright file="DataType.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Specifies data type of a column or field, for use in a <see cref="DataColum"/>.
    /// </summary>
    internal enum DataType
    {
        /// <summary>
        /// Represents a globally unique identifier (or GUID).
        /// </summary>
        Guid,
        
        /// <summary>
        /// Represents a series of characters.
        /// </summary>
        String,
        
        /// <summary>
        /// Represents a binary array.
        /// </summary>
        Binary,
        
        /// <summary>
        /// Represents a 8-bit integer.
        /// </summary>
        Bit,
        
        /// <summary>
        /// Represents a 16-bit integer.
        /// </summary>
        Short,
        
        /// <summary>
        /// Represents a 32-bit integer.
        /// </summary>
        Int,
        
        /// <summary>
        /// Represents a 64-bit integer.
        /// </summary>
        Long,
        
        /// <summary>
        /// Represents a Boolean value.
        /// </summary>        
        Boolean,
        
        /// <summary>
        /// Represents a date and time of a day.
        /// </summary>
        Datetime,
        
        /// <summary>
        /// Represents a decimal number.
        /// </summary>        
        Decimal,
        
        /// <summary>
        /// Represents a double-precision floating-point number.
        /// </summary>
        Double,
        
        /// <summary>
        /// Represents a time interval.
        /// </summary>
        Timespan
    }
}
