//-----------------------------------------------------------------------
// <copyright file="QueryCommandResult.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a query result of a command action.
    /// </summary>
    internal sealed class QueryCommandResult : CommandActionResult, IXmlSerializable
    {
        /// <summary>
        /// The query result.
        /// </summary>
        private readonly IEnumerable result;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryCommandResult"/> class.
        /// </summary>
        /// <param name="result">The query result.</param>
        public QueryCommandResult(IEnumerable result)
        {
            this.result = result;
        }

        /// <inheritdoc />
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc />
        public void ReadXml(XmlReader reader)
        {
        }

        /// <inheritdoc />
        public void WriteXml(XmlWriter writer)
        {
            /*
             * Se crea un stream para serializar las filas, el stream usa internamente un pool de buffers para reutilizar la memoria
             * y mejorar el rendimiento.
             */
            IBufferManager bufferManager = DependencyResolver.Default.Resolve<IBufferManager>();
            BufferPoolStream stream = BufferPoolStream.Create(64);
            using (QueryResultWriter rowWriter = new QueryResultWriter(stream))
            {
                foreach (var row in this.result)
                {
                    rowWriter.Write(row);
                }

                writer.WriteStartAttribute("length");
                writer.WriteValue(stream.Length);
                writer.WriteEndAttribute();

                /*
                 * Finalmente se lee todo el stream que contiene las filas serializadas.
                 */
                byte[] buffer = bufferManager.Take((int)stream.Length);
                stream.Position = 0;
                stream.Read(buffer, 0, (int)stream.Length);
                writer.WriteBase64(buffer, 0, (int)stream.Length);
                bufferManager.Return(buffer);
            }
        }
    }
}
