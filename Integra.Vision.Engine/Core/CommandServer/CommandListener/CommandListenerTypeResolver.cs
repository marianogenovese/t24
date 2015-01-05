//-----------------------------------------------------------------------
// <copyright file="CommandListenerTypeResolver.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml;

    /// <summary>
    /// Implements a resolver for data contracts.
    /// </summary>
    internal sealed class CommandListenerTypeResolver : DataContractResolver
    {
        /// <inheritdoc />
        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            return null;
        }

        /// <inheritdoc />
        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            typeName = default(XmlDictionaryString);
            typeNamespace = default(XmlDictionaryString);
            return false;
        }
    }
}
