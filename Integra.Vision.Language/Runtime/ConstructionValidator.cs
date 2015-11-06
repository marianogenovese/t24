//-----------------------------------------------------------------------
// <copyright file="ConstructionValidator.cs" company="CompanyName">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Runtime
{
    using System;
    using System.Linq.Expressions;
    using Exceptions;

    /// <summary>
    /// Observable constructor class
    /// </summary>
    internal static class ConstructionValidator
    {
        /// <summary>
        /// Validate the plan node specified.
        /// </summary>
        /// <param name="nodeType">Plan node type.</param>
        /// <param name="actualNode">Actual execution plan node.</param>
        /// <param name="left">Left expression.</param>
        /// <param name="right">Right expression</param>
        public static void Validate(PlanNodeTypeEnum nodeType, PlanNode actualNode, Expression left, Expression right)
        {
            switch (nodeType)
            {
                case PlanNodeTypeEnum.ObservableBuffer:
                    CreateBufferValidator(actualNode, left, right);
                    break;
            }
        }

        /// <summary>
        /// Validates the context of the CreateBuffer method in ObservableConstructor class.
        /// </summary>
        /// <param name="actualNode">Actual execution plan node.</param>
        /// <param name="incomingObservable">Incoming observable expression.</param>
        /// <param name="bufferTimeOrSize">Time or size expression for the buffer.</param>
        private static void CreateBufferValidator(PlanNode actualNode, Expression incomingObservable, Expression bufferTimeOrSize)
        {
            if (bufferTimeOrSize.Type.Equals(typeof(DateTime)))
            {
                throw new CompilationException("Error al compilar, el tamaño de la ventana debe ser un espacio de tiempo ('TimeSpan') y no una fecha.");
            }
        }
    }
}
