//-----------------------------------------------------------------------
// <copyright file="DispatchOperator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    
    /// <summary>
    /// This class acts like a worker which execute all the logic related to parsing, compiling and executing a script.
    /// </summary>
    internal sealed class DispatchOperator
    {
        /// <summary>
        /// The context used in the execution.
        /// </summary>
        private OperationContext context;

        /// <summary>
        /// The pipeline created for execute the operation.
        /// </summary>
        private DispatchFilter<OperationContext, OperationContext> pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchOperator"/> class.
        /// </summary>
        /// <param name="context">The context used in the operation.</param>
        /// <param name="pipeline">The pipeline created for execute the operation.</param>
        public DispatchOperator(OperationContext context, DispatchFilter<OperationContext, OperationContext> pipeline)
        {
            this.context = context;
            this.pipeline = pipeline;
        }

        /// <summary>
        /// Start the operation and execute all the logic related.
        /// </summary>
        public void Execute()
        {
            /*
             * La ejecución de la operación se realiza en 3 pasos.
             * 1. Parseo
             *    Se toma el script representado como una cadena de texto y se parsea, devolviendo asi un
             *    grupo de nodos representando los comandos incluidos en el script.
             *    Una vez ejecutado el parseo y obtenidos los nodos resultantes, dichos nodos son enviados
             *    al paso de compilación para que mediante una selección del filtro a propiado se compilen
             *    los comandos.
             *    
             * 2. Compilación
             *    La función que cumple este paso es recibir nodos resultantes del proceso de parseo, y
             *    transformar dichos nodos en comandos que internamente ya poseen la lógica especifica para
             *    calcular los argumentos y dependencias necesarias para la correcta ejecución del comando
             *    requerido.
             *    Una vez ejecutada la compilación los comandos resultantes son enviados al paso de ejecución
             *    para que mediante una serie de filtros los comandos sean validados, y ejecutados.
             * 
             * 3. Ejecución
             *    En este paso se crea mediante el patron - pipe and filters - un pipeline donde se ejecutarán
             *    distintos filtros hasta llega a la ejecución
             *   
             * NOTAS GENERALES
             * Si ocurre algún error en este paso esta clase no es responsable de transformar el error
             * sino que simplemente se limita a ejecutar los 3 pasos mencionados.
             */
            try
            {
                /*
                var enumerable = this.pipeline.Execute(null);
                var enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                }
                */
                
                /*
                foreach (string s in this.pipeline.Start(this.context))
                {
                }
                */

                this.pipeline.Start(this.context);
                this.context.Success();
            }
            catch (Exception e)
            {
                this.context.Failure(e);
            }
        }
    }
}
