//-----------------------------------------------------------------------
// <copyright file="CompileDispatchFilter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Implements the logic of compiling in which receive a node and return a command as result of the compiling.
    /// </summary>
    internal sealed class CompileDispatchFilter : DispatchFilter<IParseFilterContext, ICompileFilterContext>
    {
        /*
         * Este filtro implementa el paso de transformar los nodos que contiene el contexto de parseo y
         * con esos nodos construir los comandos que se intentan ejecutar en el script.
         */

        /// <inheritdoc />
        public override ICompileFilterContext Execute(IParseFilterContext context)
        {
            DispatchContext dispatchContext = context as DispatchContext;

            /*
             * Aqui se debe tomar los nodos y convertirlos a comandos
             * en la clase DispatchContext agregar un elemento al diccionario de Data con los comandos.
             */
            return context as ICompileFilterContext;
        }
    }
}
