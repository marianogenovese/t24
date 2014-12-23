//-----------------------------------------------------------------------
// <copyright file="InternalDataColumnCollection.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Implements a collection which can be a by key dictionary or by index collection.
    /// </summary>
    internal sealed class InternalDataColumnCollection : KeyedCollection<string, DataColumn>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalDataColumnCollection"/> class.
        /// </summary>
        public InternalDataColumnCollection() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <inheritdoc />
        protected override string GetKeyForItem(DataColumn item)
        {
            return item.Name;
        }
    }
}
