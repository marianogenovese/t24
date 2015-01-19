//-----------------------------------------------------------------------
// <copyright file="ParseDispatchFilter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;

    /// <summary>
    /// Implements the parsing process in which receive a string and return a node as result of the parsing.
    /// </summary>
    internal sealed class ParseDispatchFilter : DispatchFilter<DispatchContext, IParseFilterContext>
    {
        /*
         * Este filtro implementa el paso de parsear el script que viene en el request y construir una serie de nodos
         * que presentan a los comandos que se han parseado, se retorna un contexto de parseo que debe contener los
         * nodos creados.
         */

        /// <inheritdoc />
        public override IParseFilterContext Execute(DispatchContext context)
        {
            /*
             * Aqui se debe tomar el script que esta en context.OperationContext.Request.Script
             * parsearlo y en la clase DispatchContext agregar un elemento al diccionario de Data.
             */

            try
            {
                if (context.Data["IsPrivateCommand"].Equals(true))
                {
                    PrivateParser parser = new PrivateParser(context.Request.Script);
                    context.Nodes = parser.Parse();
                }
                else
                {
                    PublicParser parser = new PublicParser(context.Request.Script);
                    context.Nodes = parser.Parse();
                }
            }
            catch (System.Exception e)
            {
                throw new InterpretationException(e.ToString());
            }

            return context;
        }
    }
}
