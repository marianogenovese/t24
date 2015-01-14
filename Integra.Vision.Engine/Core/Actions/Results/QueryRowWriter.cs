//-----------------------------------------------------------------------
// <copyright file="QueryRowWriter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    
    /// <summary>
    /// Implements a row writer for a result of a query execution.
    /// </summary>
    internal sealed class QueryRowWriter
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
        /// The value writer.
        /// </summary>
        private readonly QueryRowValueWriter valueWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryRowWriter"/> class.
        /// </summary>
        /// <param name="output">The output stream.</param>
        public QueryRowWriter(Stream output)
        {
            this.output = output;
            this.writer = new BinaryWriter(output);
            this.valueWriter = new QueryRowValueWriter(output);
        }

        /// <summary>
        /// Writes a object in represents of a row.
        /// </summary>
        /// <param name="row">The object that represents the row.</param>
        public void Write(object row)
        {
            // Guardo posicion donde se escribirá la longitud de la fila.
            long lengthPosition = this.output.Position;

            // Pre-escribo la longitud con 0.
            this.writer.Write((long)0);

            // Guardo la posicion inicial donde se escribirá los datos de la fila.
            long startPosition = this.output.Position;
            
            // Escribo la fila.
            this.WriteValues(row);

            // Guardo la posicion final para calcula la longitud.
            long endPosition = this.output.Position;

            // Calculo la longitud de la fila.
            long rowLength = endPosition - startPosition;

            // Muevo la posicion a la posicion donde debe escribir la longitud de la fila.
            this.output.Position = lengthPosition;
            
            // Escribo la longitud de la fila.
            this.writer.Write(rowLength);

            // Regreso a la posicion final de la fila para que siga escribiendo las demas filas.
            this.output.Position = endPosition;
        }

        /// <summary>
        /// Use reflection to serialize the properties of an object.
        /// </summary>
        /// <param name="o">The object to serialize.</param>
        private void WriteValues(object o)
        {
            PropertyInfo[] properties = o.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance).Where(info => info is PropertyInfo).Cast<PropertyInfo>().ToArray();

            // Escribo el numero de campos que tiene la fila.
            this.writer.Write(properties.Length);

            foreach (PropertyInfo property in properties)
            {
                this.valueWriter.Write(property.Name, property.GetValue(o, null));
            }
        }
    }
}
