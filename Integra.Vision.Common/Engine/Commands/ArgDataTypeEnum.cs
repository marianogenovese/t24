//-----------------------------------------------------------------------
// <copyright file="ArgDataTypeEnum.cs" company="Integra.Vision.Common">
//     Copyright (c) Integra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// Data type argument
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum ArgDataTypeEnum
    {
        /// <summary>
        /// string type
        /// </summary>
        String = 1,

        /// <summary>
        /// boolean type
        /// </summary>
        Bool = 2,

        /// <summary>
        /// character type
        /// </summary>
        Char = 3,

        /// <summary>
        /// byte type
        /// </summary>
        Byte = 4,

        /// <summary>
        /// SByte type
        /// </summary>
        SByte = 5,

        /// <summary>
        /// short type
        /// </summary>
        Short = 6,

        /// <summary>
        /// UShort type
        /// </summary>
        UShort = 7,

        /// <summary>
        /// integer type
        /// </summary>
        Int = 8,

        /// <summary>
        /// UInteger type
        /// </summary>
        UInt = 9,

        /// <summary>
        /// long type
        /// </summary>
        Long = 10,

        /// <summary>
        /// ULong type
        /// </summary>
        ULong = 11,

        /// <summary>
        /// float type
        /// </summary>
        Float = 12,

        /// <summary>
        /// double type
        /// </summary>
        Double = 13,

        /// <summary>
        /// DateTime type
        /// </summary>
        DateTime = 14,

        /// <summary>
        /// object type
        /// </summary>
        Object = 15,

        /// <summary>
        /// user defined data type
        /// </summary>
        UserDefinedDataType = 16,
  
        /// <summary>
        /// decimal type
        /// </summary>
        Decimal = 17
    }
}
