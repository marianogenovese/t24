//-----------------------------------------------------------------------
// <copyright file="QueryRowValueWriter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.IO;
    using System.Text;
    
    /// <summary>
    /// Implements a row value writer for a result of a query execution.
    /// </summary>
    internal sealed class QueryRowValueWriter
    {
        /// <summary>
        /// Row output.
        /// </summary>
        private readonly Stream output;

        /// <summary>
        /// The writer of values.
        /// </summary>
        private readonly BinaryWriter writer;

        /// <summary>
        /// The encoding used for strings.
        /// </summary>
        private readonly Encoding encoding;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryRowValueWriter"/> class.
        /// </summary>
        /// <param name="output">The output stream.</param>
        public QueryRowValueWriter(Stream output)
        {
            this.output = output;
            this.writer = new BinaryWriter(output);
            this.encoding = new UTF8Encoding(false, true);
        }

        /// <summary>
        /// Try to write the value into the stream.
        /// </summary>
        /// <param name="name">The name of the value to serialize.</param>
        /// <param name="value">The value to serialize.</param>
        public void Write(string name, object value)
        {
            /*
             * El formato de un campo es:
             * Longitud del nombre del campo (4B)
             * Nombre del campo (Var)
             * Tiene valor? (1B)
             * Tipo de dato (1B)
             * Longitud del valor del campo. (8B)
             * Valor del campo. (Var)
             */

            // Escribo el nombre del campo.
            // Primero la longitud del nombre.
            this.writer.Write(this.encoding.GetByteCount(name));

            // Despues el nombre del campo.
            this.writer.Write(this.encoding.GetBytes(name));

            // Tiene valor?
            if (value == null || Type.GetTypeCode(value.GetType()) == TypeCode.DBNull)
            {
                // Flag indicando si tiene valor
                this.writer.Write((byte)0);

                // Tipo de dato es NULL.
                this.writer.Write((byte)0);
                return;
            }
            else
            {
                this.writer.Write((byte)1);
            }

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Boolean:
                    this.writer.Write((byte)1);
                    break;

                case TypeCode.Char:
                    this.writer.Write((byte)2);
                    break;

                case TypeCode.SByte:
                    this.writer.Write((byte)3);
                    break;

                case TypeCode.Byte:
                    this.writer.Write((byte)4);
                    break;

                case TypeCode.Int16:
                    this.writer.Write((byte)5);
                    break;

                case TypeCode.UInt16:
                    this.writer.Write((byte)6);
                    break;

                case TypeCode.Int32:
                    this.writer.Write((byte)7);
                    break;

                case TypeCode.UInt32:
                    this.writer.Write((byte)8);
                    break;

                case TypeCode.Int64:
                    this.writer.Write((byte)9);
                    break;

                case TypeCode.UInt64:
                    this.writer.Write((byte)10);
                    break;

                case TypeCode.Single:
                    this.writer.Write((byte)11);
                    break;

                case TypeCode.Double:
                    this.writer.Write((byte)12);
                    break;

                case TypeCode.Decimal:
                    this.writer.Write((byte)13);
                    break;

                case TypeCode.DateTime:
                    this.writer.Write((byte)14);
                    break;

                case TypeCode.String:
                    this.writer.Write((byte)15);
                    break;

                default:
                    if (value is byte[])
                    {
                        this.writer.Write((byte)16);
                    }

                    if (value is Guid)
                    {
                        this.writer.Write((byte)17);
                    }

                    break;
            }

            // Se calcula la longitud del valor.
            long lengthPosition = this.output.Position;
            
            // Se escribe la longitud del campo, primero en 0 para que posteriormente se escriba la longitud.
            this.writer.Write((long)0);

            // La posición inicial del valor del campo
            long startPosition = this.output.Position;

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Boolean:
                    this.writer.Write((bool)value);
                    break;

                case TypeCode.Char:
                    this.writer.Write((char)value);
                    break;

                case TypeCode.SByte:
                    this.writer.Write((sbyte)value);
                    break;

                case TypeCode.Byte:
                    this.writer.Write((byte)value);
                    break;

                case TypeCode.Int16:
                    this.writer.Write((short)value);
                    break;

                case TypeCode.UInt16:
                    this.writer.Write((ushort)value);
                    break;

                case TypeCode.Int32:
                    this.writer.Write((int)value);
                    break;

                case TypeCode.UInt32:
                    this.writer.Write((uint)value);
                    break;

                case TypeCode.Int64:
                    this.writer.Write((long)value);
                    break;

                case TypeCode.UInt64:
                    this.writer.Write((ulong)value);
                    break;

                case TypeCode.Single:
                    this.writer.Write((float)value);
                    break;

                case TypeCode.Double:
                    this.writer.Write((double)value);
                    break;

                case TypeCode.Decimal:
                    this.writer.Write((decimal)value);
                    break;

                case TypeCode.DateTime:
                    this.writer.Write(((DateTime)value).ToBinary());
                    break;

                case TypeCode.String:
                    this.writer.Write(this.encoding.GetBytes((string)value));
                    break;

                default:
                    if (value is byte[])
                    {
                        this.writer.Write((byte[])value);
                    }

                    if (value is Guid)
                    {
                        this.writer.Write(((Guid)value).ToByteArray());
                    }

                    break;
            }
            
            // Se toma la posicion final.
            long endPosition = this.output.Position;
            
            // Se calcula la longitud del campo.
            long length = endPosition - startPosition;
            
            // Se mueve a la posición inicial donde estan los bytes de la longitud.
            this.output.Position = lengthPosition;
            
            // Se escribe la longitud del campo.
            this.writer.Write(length);
            
            // Se mueve a la posición original para que continue con el resto de los valores.
            this.output.Position = endPosition;
        }
    }
}
