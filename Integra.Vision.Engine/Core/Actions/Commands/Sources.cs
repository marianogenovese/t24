//-----------------------------------------------------------------------
// <copyright file="Sources.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using System.Threading.Tasks.Schedulers;

    /// <summary>
    /// TPL source test
    /// </summary>
    internal static class Sources
    {
        /// <summary>
        /// Doc goes here.
        /// </summary>
        private static Dictionary<string, BroadcastBlock<Event.EventObject>> dictionaryOfBroadcastBlock = new Dictionary<string, BroadcastBlock<Event.EventObject>>();

        /// <summary>
        /// BroadcastBlock source.
        /// </summary>
        private static BroadcastBlock<Event.EventObject> broadcastBlock;

        /// <summary>
        /// Gets the broadcast block source.
        /// </summary>
        public static BroadcastBlock<Event.EventObject> BroadcastBlock
        {
            get
            {
                if (broadcastBlock == null)
                {
                    broadcastBlock = new BroadcastBlock<Event.EventObject>(
                        t => t,
                        new DataflowBlockOptions() { BoundedCapacity = DataflowBlockOptions.Unbounded });
                    broadcastBlock.LinkTo(DataflowBlock.NullTarget<Event.EventObject>());
                }

                return broadcastBlock;
            }
        }

        /// <summary>
        /// Creates a new source.
        /// </summary>
        /// <param name="name">Name of the source</param>
        public static void StartSource(string name)
        {
            if (!dictionaryOfBroadcastBlock.ContainsKey(name))
            {
                BroadcastBlock<Event.EventObject> bcb = new BroadcastBlock<Event.EventObject>(
                    t => t,
                    new DataflowBlockOptions() { BoundedCapacity = DataflowBlockOptions.Unbounded });
                bcb.LinkTo(DataflowBlock.NullTarget<Event.EventObject>());

                dictionaryOfBroadcastBlock.Add(name, bcb);
            }
        }

        /// <summary>
        /// Deletes the specify source.
        /// </summary>
        /// <param name="name">Name of the source</param>
        public static void StopSource(string name)
        {
            if (dictionaryOfBroadcastBlock.ContainsKey(name))
            {
                dictionaryOfBroadcastBlock[name].Complete();
                dictionaryOfBroadcastBlock.Remove(name);
            }
        }

        /// <summary>
        /// Gets the specify source.
        /// </summary>
        /// <param name="name">Name of the source</param>
        /// <returns>The BroadcastBlock source</returns>
        public static BroadcastBlock<Event.EventObject> GetSource(string name)
        {
            if (dictionaryOfBroadcastBlock.ContainsKey(name))
            {
                return dictionaryOfBroadcastBlock[name];
            }

            return null;
        }
    }
}
