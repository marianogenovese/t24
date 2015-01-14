//-----------------------------------------------------------------------
// <copyright file="QueryResultWriter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.IO;
    
    /// <summary>
    /// Implements a serializer of a query result.
    /// </summary>
    internal sealed class QueryResultWriter : IDisposable
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
        /// The row serializer.
        /// </summary>
        private readonly QueryRowWriter rowWriter;
        
        /// <summary>
        /// The counter of rows.
        /// </summary>
        private int rowCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryResultWriter"/> class.
        /// </summary>
        /// <param name="output">The output stream.</param>
        public QueryResultWriter(Stream output)
        {
            this.output = output;
            this.writer = new BinaryWriter(this.output);
            this.rowWriter = new QueryRowWriter(this.output);

            // Obtengo la posición actual
            long startPosition = this.output.Position == 0 ? 4 : this.output.Position;

            // Muevo a la posicion inicial del stream
            this.output.Position = 0;

            // Aumento el numero de filas y escribo ese nuevo numero
            this.writer.Write(0);

            // Retorno la posicion a la inicial antes de escribir el contador.
            this.output.Position = startPosition;
        }

        /// <summary>
        /// Writes a object in represents of a row.
        /// </summary>
        /// <param name="row">The object that represents the row.</param>
        public void Write(object row)
        {
            // Obtengo la posición actual
            long startPosition = this.output.Position == 0 ? 4 : this.output.Position;
            
            // Muevo a la posicion inicial del stream
            this.output.Position = 0;
            
            // Aumento el numero de filas y escribo ese nuevo numero
            this.writer.Write(++this.rowCount);
            
            // Retorno la posicion a la inicial antes de escribir el contador.
            this.output.Position = startPosition;
            
            // Escribo la fila.
            this.rowWriter.Write(row);
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            this.output.Dispose();
        }
    }
}
