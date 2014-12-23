//-----------------------------------------------------------------------
// <copyright file="RuntimeObjectWrapper.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Delegate used for dynamic invocation.
    /// </summary>
    /// <param name="target">The object used as invocation target.</param>
    /// <param name="args">The invocation arguments.</param>
    /// <returns>The value that return the invocation.</returns>
    public delegate object DynamicMethodDelegate(object target, object[] args);

    /// <summary>
    /// This class implements a wrapper for invoke method of an instance object.
    /// </summary>
    internal class RuntimeObjectWrapper : IDisposable
    {
        /// <summary>
        /// Instance of the object to be used.
        /// </summary>
        private object instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeObjectWrapper" /> class.
        /// </summary>
        /// <param name="instance">The instance of the object to be used.</param>
        public RuntimeObjectWrapper(object instance)
        {
            this.instance = instance;
        }

        /// <summary>
        /// Gets the instance associated with the wrapper.
        /// </summary>
        protected object Instance
        {
            get
            {
                return this.instance;
            }
        }

        /// <summary>
        /// Free resources relates to the wrapper and their object instance.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Try to invoke a method with no return value.
        /// </summary>
        /// <param name="methodName">The Method name.</param>
        /// <param name="args">The Method arguments.</param>
        /// <returns>true if the invocation was completed successfully; otherwise, false.</returns>
        protected bool TryInvokeAction(string methodName, params object[] args)
        {
            MethodInfo method = default(MethodInfo);
            if (!this.TryGetMethodInfo(methodName, this.GetTypes(args), out method))
            {
                return false;
            }

            try
            {
                DynamicMethodDelegate wrapper = this.Create(method);
                wrapper.Invoke(this.instance, args);
                return true;
            }
            catch (Exception e)
            {
                // Diagnostics.DiagnosticHelper.Logger.Warning(Diagnostics.DiagnosticsEventIds.RuntimeObjectWrapperInvocation, Resources.SR.RuntimeObjectWrapperInvocationException(this.instance.GetType().FullName, methodName, e.ToString()));
            }

            return false;
        }

        /// <summary>
        /// Try to invoke a method with return value.
        /// </summary>
        /// <param name="methodName">The Method name.</param>
        /// <param name="returnValue">When this method returns, contains the return value of the method invocation if the method is found; otherwise, the default value for the type of the returnValue parameter. This parameter is passed uninitialized.</param>
        /// <param name="args">The Method arguments.</param>
        /// <returns>true if the invocation was completed successfully; otherwise, false.</returns>
        protected virtual bool TryInvokeFunction(string methodName, out object returnValue, params object[] args)
        {
            returnValue = default(object);
            MethodInfo method = default(MethodInfo);
            if (!this.TryGetMethodInfo(methodName, this.GetTypes(args), out method))
            {
                return false;
            }

            try
            {
                DynamicMethodDelegate wrapper = this.Create(method);
                returnValue = wrapper.Invoke(this.instance, args);
                return true;
            }
            catch (Exception e)
            {
                // Diagnostics.DiagnosticHelper.Logger.Warning(Diagnostics.DiagnosticsEventIds.RuntimeObjectWrapperInvocation, Resources.SR.RuntimeObjectWrapperInvocationException(this.instance.GetType().FullName, methodName, e.ToString()));
            }

            return false;
        }

        /// <summary>
        /// Try to invoke a method with return value.
        /// </summary>
        /// <param name="genericTypes">The types used for generic method call.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="returnValue">When this method returns, contains the return value of the method invocation if the method is found; otherwise, the default value for the type of the returnValue parameter. This parameter is passed uninitialized.</param>
        /// <param name="args">Method arguments.</param>
        /// <returns>true if the invocation was completed successfully; otherwise, false.</returns>
        protected bool TryInvokeGenericFunction(Type[] genericTypes, string methodName, out object returnValue, params object[] args)
        {
            returnValue = default(object);

            MethodInfo method = default(MethodInfo);
            if (!this.TryGetMethodInfo(methodName, this.GetTypes(args), out method))
            {
                return false;
            }

            try
            {
                DynamicMethodDelegate wrapper = this.Create(method);
                returnValue = wrapper.Invoke(this.instance, args);
                return true;
            }
            catch (Exception e)
            {
                // Diagnostics.DiagnosticHelper.Logger.Warning(Diagnostics.DiagnosticsEventIds.RuntimeObjectWrapperInvocation, Resources.SR.RuntimeObjectWrapperInvocationException(this.instance.GetType().FullName, methodName, e.ToString()));
            }

            return false;
        }

        /// <summary>
        /// Try to find the method based on name and arguments type.
        /// </summary>
        /// <param name="methodName">The method name.</param>
        /// <param name="argsTypes">The arguments.</param>
        /// <param name="method">When this method returns, contains the method information if the method is found; otherwise, the default value for the type of the method parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the method exists; otherwise, false.</returns>
        protected virtual bool TryGetMethodInfo(string methodName, Type[] argsTypes, out MethodInfo method)
        {
            method = default(MethodInfo);
            method = this.instance.GetType().GetMethod(methodName, argsTypes);
            return method != null ? true : false;
        }

        /// <summary>
        /// Get the type of each values and return an array with the types.
        /// </summary>
        /// <param name="values">The values to get the type.</param>
        /// <returns>An array of types.</returns>
        protected Type[] GetTypes(object[] values)
        {
            Contract.Requires(values != null);
            Type[] types = new Type[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                types[i] = values[i].GetType();
            }

            return types;
        }

        /// <summary>
        /// Generates a DynamicMethodDelegate delegate from a MethodInfo object.
        /// </summary>
        /// <param name="method">The method used for generate a new invocation delegate.</param>
        /// <returns>the invocation delegate</returns>
        private DynamicMethodDelegate Create(MethodInfo method)
        {
            ParameterInfo[] parms = method.GetParameters();
            int numparams = parms.Length;

            Type[] argTypes = { typeof(object), typeof(object[]) };

            // Create dynamic method and obtain its IL generator to
            // inject code.
            DynamicMethod dynam = new DynamicMethod(string.Empty, typeof(object), argTypes, typeof(RuntimeObjectWrapper));
            ILGenerator il = dynam.GetILGenerator();

            // Define a label for succesfull argument count checking.
            Label argsOK = il.DefineLabel();

            // Check input argument count.
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldlen);
            il.Emit(OpCodes.Ldc_I4, numparams);
            il.Emit(OpCodes.Beq, argsOK);

            // Argument count was wrong, throw TargetParameterCountException.
            il.Emit(OpCodes.Newobj, typeof(TargetParameterCountException).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Throw);

            // Mark IL with argsOK label.
            il.MarkLabel(argsOK);

            // If method isn't static push target instance on top
            // of stack.
            if (!method.IsStatic)
            {
                // Argument 0 of dynamic method is target instance.
                il.Emit(OpCodes.Ldarg_0);
            }

            // Lay out args array onto stack.
            int i = 0;
            while (i < numparams)
            {
                // Push args array reference onto the stack, followed
                // by the current argument index (i). The Ldelem_Ref opcode
                // will resolve them to args[i].

                // Argument 1 of dynamic method is argument array.
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);

                // If parameter [i] is a value type perform an unboxing.
                Type parmType = parms[i].ParameterType;
                if (parmType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, parmType);
                }

                i++;
            }

            // Perform actual call.
            // If method is not final a callvirt is required
            // otherwise a normal call will be emitted.
            if (method.IsFinal)
            {
                il.Emit(OpCodes.Call, method);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, method);
            }

            if (method.ReturnType != typeof(void))
            {
                // If result is of value type it needs to be boxed
                if (method.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Box, method.ReturnType);
                }
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }

            // Emit return opcode.
            il.Emit(OpCodes.Ret);

            return (DynamicMethodDelegate)dynam.CreateDelegate(typeof(DynamicMethodDelegate));
        }
    }
}
