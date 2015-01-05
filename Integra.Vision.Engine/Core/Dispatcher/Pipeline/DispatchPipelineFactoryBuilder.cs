//-----------------------------------------------------------------------
// <copyright file="DispatchPipelineFactoryBuilder.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    
    /// <summary>
    /// Allow to build dispatch pipeline factories based on configured steps.
    /// </summary>
    internal sealed class DispatchPipelineFactoryBuilder
    {
        /// <summary>
        /// Construct, based on configuration, a factory for create new dispatch pipelines.
        /// </summary>
        /// <returns>A factory for create new pipelines.</returns>
        public DispatchFilterFactory<OperationContext, OperationContext> Build()
        {
            object lastInstance = new BeginDispatchFilterFactory();

            /*
             * En este metodo se resuelvan todas la depedencias en forma de instancias de fabricas de filtros
             * y mediante wrapper invocar el metodo next para encadenar las fabricas para que finalmente se llame
             * al metodo create para devolver una fabrica
             */

            RuntimeDispatchPipelineFactoryWrapper factory;

            foreach (object instance in DispatchPipelineFactoryDependencyResolver.Instance.ResolveFactories())
            {
                if (lastInstance != null)
                {
                    factory = new RuntimeDispatchPipelineFactoryWrapper(lastInstance);
                    object newInstance = default(object);
                    if (!factory.TryNext(instance, out newInstance))
                    {
                        throw new InvalidOperationException();
                    }
                    
                    lastInstance = newInstance;
                }
                else
                {
                    lastInstance = instance;
                }
            }

            factory = new RuntimeDispatchPipelineFactoryWrapper(lastInstance);
            object endFilterDispatchFactoryInstance = default(object);
            if (!factory.TryNext(new EndDispatchFilterFactory(), out endFilterDispatchFactoryInstance))
            {
                throw new InvalidOperationException();
            }

            lastInstance = endFilterDispatchFactoryInstance;

            // Se hace un casteo que aunque es sensible y algo pesado se hace una sola vez
            // ya se tiene lista la fabrica.
            return lastInstance as DispatchFilterFactory<OperationContext, OperationContext>;
        }

        /// <summary>
        /// Provides a runtime wrapper for create new dispatch pipeline filters.
        /// </summary>
        private class RuntimeDispatchPipelineFactoryWrapper : RuntimeObjectWrapper
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RuntimeDispatchPipelineFactoryWrapper"/> class.
            /// </summary>
            /// <param name="instance">The real object.</param>
            public RuntimeDispatchPipelineFactoryWrapper(object instance) : base(instance)
            {
            }
            
            /// <summary>
            /// Try to link filters.
            /// </summary>
            /// <param name="next">The next filter.</param>
            /// <param name="result">The created filter as result of the linking</param>
            /// <returns>true if the invocation was completed successfully; otherwise, false.</returns>
            public bool TryNext(object next, out object result)
            {
                result = default(object);
                return this.TryInvokeFunction("Next", out result, next);
            }

            /// <inheritdoc />
            protected override bool TryGetMethodInfo(string methodName, Type[] argsTypes, out MethodInfo method)
            {
                // Sobreescribimos el metodo para devolver el metodo next pues es algo complicado de encontrar.
                if (string.Equals("Next", methodName, StringComparison.OrdinalIgnoreCase))
                {
                    // Ojo estas lineas son bastante delicadas seguramente en el futuro se verá la mejor manera de
                    method = this.Instance.GetType().GetMethods().Where((info) => string.Equals("Next", info.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    method = method.MakeGenericMethod(argsTypes[0].GetDispatchFilterFactoryArgs()[1]);
                    return method != null;
                }
                else
                {
                    return base.TryGetMethodInfo(methodName, argsTypes, out method);
                }
            }

            /// <inheritdoc />
            protected override bool TryInvokeFunction(string methodName, out object returnValue, params object[] args)
            {
                returnValue = default(object);
                if (string.Equals("Next", methodName, StringComparison.OrdinalIgnoreCase))
                {
                    MethodInfo method = default(MethodInfo);
                    if (this.TryGetMethodInfo(methodName, this.GetTypes(args), out method))
                    {
                        returnValue = method.Invoke(this.Instance, args);
                        return true;
                    }

                    return false;
                }
                else
                {
                    return base.TryInvokeFunction(methodName, out returnValue, args);
                }
            }
        }
    }
}