//-----------------------------------------------------------------------
// <copyright file="EventTypesEnum.cs" company="Ingetra.Vision.Event">
//     Copyright (c) Ingetra.Vision.Event. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.EventObject
{
    using System;

    /// <summary>
    /// Event types enumerator
    /// </summary>
    [Serializable]
    public enum EventTypesEnum
    {
        /// <summary>
        /// Normal event type
        /// </summary>
        Normal = 10,

        /// <summary>
        /// Timeout event type
        /// </summary>
        TimeOut = 20,

        /// <summary>
        /// Error event type
        /// </summary>
        Error = 30,
    }
}
