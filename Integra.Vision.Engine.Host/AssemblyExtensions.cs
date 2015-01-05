//-----------------------------------------------------------------------
// <copyright file="AssemblyExtensions.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    
    /// <summary>
    /// Extensions used for extract the service product
    /// </summary>
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// Get the product information
        /// </summary>
        /// <param name="assembly">Target assembly to extract the product information</param>
        /// <returns>product information.</returns>
        public static string GetProductString(this Assembly assembly)
        {
            StringBuilder sb = new StringBuilder();

            string assemblyProduct = FromAttribute<AssemblyProductAttribute>(assembly).Product;

            string assemblyCopyright = FromAttribute<AssemblyCopyrightAttribute>(assembly).Copyright;

            if (string.IsNullOrWhiteSpace(assemblyCopyright))
            {
                assemblyCopyright = FromAttribute<AssemblyCompanyAttribute>(assembly).Company;
            }

            string assemblyDescription = FromAttribute<AssemblyDescriptionAttribute>(assembly).Description ?? assembly.GetName().Name;
            var version = FromAttribute<AssemblyFileVersionAttribute>(assembly).Version;

            sb.AppendFormat("{0} v{1}", assemblyDescription, version);
            sb.AppendLine();
            sb.AppendLine(assemblyProduct);
            sb.AppendLine(assemblyCopyright);

            return sb.ToString();
        }
        
        /// <summary>
        /// Find and return the first or default custom argument
        /// </summary>
        /// <typeparam name="T">Type of attribute to find in the assembly</typeparam>
        /// <param name="assembly">Target assembly</param>
        /// <returns>If the attribute exists in the target assembly returns it; otherwise, null.</returns>
        private static T FromAttribute<T>(Assembly assembly) where T : Attribute
        {
            return (assembly.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T) ?? Activator.CreateInstance<T>();
        }
    }
}
