//-----------------------------------------------------------------------
// <copyright file="RuntimeDependencyResolver.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Integra.Vision.Dependency;

    /// <summary>
    /// Runtime dependency resolver.
    /// </summary>
    internal static class RuntimeDependencyResolver
    {
        /// <summary>
        /// Mutual exclusive lock used for initialization.
        /// </summary>
        private static readonly object LockThis = new object();

        /// <summary>
        /// Initialization flag.
        /// </summary>
        private static bool hasInitialized = false;

        /// <summary>
        /// List of loaded assemblies from base path.
        /// </summary>
        private static Assembly[] assembliesCache;

        /// <summary>
        /// List of loaded assemblies from base path.
        /// </summary>
        private static Dictionary<string, Type> typesCache = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Try to resolve a reference type from the current AppDomain.
        /// </summary>
        /// <param name="type">The alias name registered in the host configuration.</param>
        /// <param name="instance">When this method returns, contains the value associated with the specified type name, if the type is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the request type exists; otherwise, false.</returns>
        public static bool TryResolve(string type, out object instance)
        {
            lock (LockThis)
            {
                if (!hasInitialized)
                {
                    Initialize(RuntimeConfiguration.Current);
                    hasInitialized = true;
                }
            }

            instance = default(object);

            Type requestedType = default(Type);

            if (!TryResolveType(type, out requestedType))
            {
                return false;
            }

            return TryCreateInstanceOf(requestedType, out instance);
        }

        /// <summary>
        /// Try to resolve a reference type from the current AppDomain.
        /// </summary>
        /// <param name="dependencyResolver">Used for resolve the requested type.</param>
        /// <param name="registeredType">The alias name registered in the host configuration.</param>
        /// <param name="instance">When this method returns, contains the value associated with the specified type name, if the type is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the request type exists; otherwise, false.</returns>
        public static bool TryResolve(IDependencyResolver dependencyResolver, string registeredType, out object instance)
        {
            lock (LockThis)
            {
                if (!hasInitialized)
                {
                    Initialize(RuntimeConfiguration.Current);
                    hasInitialized = true;
                }
            }

            instance = default(object);

            Type requestedType = default(Type);

            if (!TryResolveType(registeredType, out requestedType))
            {
                return false;
            }

            return TryCreateInstanceOf(requestedType, out instance);
        }

        /// <summary>
        /// Check if a particular type pair has been registered.
        /// </summary>
        /// <param name="typeName">Type to check registration for.</param>
        /// <returns>True if this type has been registered, false if not.</returns>
        public static bool CanResolveType(string typeName)
        {
            bool canResolve = false;

            if (typesCache.ContainsKey(typeName))
            {
                canResolve = true;
                goto resolve;
            }

            // Se busca por el AssemblyQualifiedName pero si no lo encuentra 
            Type requestedType = assembliesCache.SelectMany(s => s.GetTypes()).Where(p => string.Equals(p.AssemblyQualifiedName, typeName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            if (requestedType != null)
            {
                typesCache.Add(typeName, requestedType);
                canResolve = true;
            }

        resolve:
            return canResolve;
        }

        /// <summary>
        /// Try to resolve a reference type from the current AppDomain.
        /// </summary>
        /// <param name="typeName">Assembly full name of type to get from the resolver.</param>
        /// <param name="requestedType">When this method returns, contains the value associated with the specified type name, if the type is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the request type exists; otherwise, false.</returns>
        public static bool TryResolveType(string typeName, out Type requestedType)
        {
            requestedType = default(Type);
            if (CanResolveType(typeName))
            {
                requestedType = typesCache[typeName];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Runtime dependency initialization.
        /// </summary>
        /// <param name="configuration">Configuration for the runtime.</param>
        private static void Initialize(RuntimeConfiguration configuration)
        {
            LoadAssemblies();
        }

        /// <summary>
        /// Load to the AppDomain the assemblies found in the folder.
        /// </summary>
        private static void LoadAssemblies()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            List<Assembly> assembliesList = new List<Assembly>();
            Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.FileEnumeration, "Loading assemblies from {0}", RuntimeConfiguration.Current.BasePath);
            DirectoryInfo directory = new DirectoryInfo(RuntimeConfiguration.Current.BasePath);
            foreach (FileInfo file in directory.EnumerateFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                Diagnostics.DiagnosticHelper.Logger.Debug(Diagnostics.DiagnosticsEventIds.DependencyAssemblyLoading, "Loading file {0}...", file.Name);
                try
                {
                    Assembly assembly = Assembly.Load(File.ReadAllBytes(file.FullName));
                    assembliesList.Add(assembly);

                    // AppDomain.CurrentDomain.Load(File.ReadAllBytes(file.FullName));
                }
                catch (BadImageFormatException e)
                {
                    // Es solo un warning aqui
                    Diagnostics.DiagnosticHelper.Logger.Warning(Diagnostics.DiagnosticsEventIds.DependencyAssemblyLoading, "File {0} is not a valid image.", file.FullName);
                }
                catch (Exception e)
                {
                    throw new RuntimeException(Resources.SR.FileLoadError(file.FullName), e);
                }
            }

            // assembliesCache = AppDomain.CurrentDomain.GetAssemblies(); // assembliesList.ToArray();
            assembliesCache = assembliesList.ToArray();
        }

        /// <summary>
        /// For assembly resolution in the AppDomain
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The arguments</param>
        /// <returns>The assembly resolved</returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyName = args.Name;

            Assembly resolvedAssembly = assembliesCache.Where(assembly => string.Equals(assembly.FullName, assemblyName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (resolvedAssembly == null)
            {
                resolvedAssembly = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => string.Equals(assembly.FullName, assemblyName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            }

            if (resolvedAssembly == null)
            {
                resolvedAssembly = args.RequestingAssembly;
            }

            return resolvedAssembly;
        }

        /// <summary>
        /// Create a instance of the requested type.
        /// </summary>
        /// <param name="requestedType">The requested type to create.</param>
        /// <param name="instance">When this method returns, contains the instance requested associated with the specified type, if the type is found; otherwise, the default value for the instance parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the request type exists; otherwise, false.</returns>
        private static bool TryCreateInstanceOf(Type requestedType, out object instance)
        {
            instance = default(object);
            bool canCreateInstanceOf = false;
            try
            {
                Type requestedImplementation = assembliesCache.SelectMany(s => s.GetTypes()).Where(p => requestedType.IsAssignableFrom(p) && p.IsClass && !p.IsInterface).SingleOrDefault();
                if (requestedImplementation != null)
                {
                    instance = Activator.CreateInstance(requestedImplementation);
                    canCreateInstanceOf = true;
                }
            }
            catch (Exception e)
            {
                Diagnostics.DiagnosticHelper.Logger.Warning(Diagnostics.DiagnosticsEventIds.DependencyInstanceCreation, e.ToString());
            }

            return canCreateInstanceOf;
        }
    }
}
