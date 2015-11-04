//-----------------------------------------------------------------------
// <copyright file="ReceiveCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using System.Threading.Tasks.Dataflow;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Event;
    using Integra.Vision.Language;
    using Integra.Vision.Language.Runtime;
    using System.Threading.Tasks;

    /// <summary>
    /// Implements all the process of return the results to the client.
    /// </summary>
    internal sealed class ReceiveCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Transaction counter
        /// </summary>
        public long contador = 0;

        /// <summary>
        /// Transaction counter
        /// </summary>
        public static long counter = 0;

        /// <summary>
        /// Doc goes here
        /// </summary>
        public static Timer t = new Timer(timerCallback, null, 0, 1000);

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                this.CreateEvent(command as ReceiveCommand);
                return new OkCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains the logic for return results to the client.
        /// </summary>
        /// <param name="receiveCommand">Receive command</param>
        private void CreateEvent(ReceiveCommand receiveCommand)
        {
            string onCondition = string.Empty;
            string whereCondition = string.Empty;
            string projectionScript = string.Empty;

            using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
            {
                // create repository
                Repository<Database.Models.UserDefinedObject> repoUserDefinedObject = new Repository<Database.Models.UserDefinedObject>(context);
                Repository<Database.Models.PList> repoProjection = new Repository<Database.Models.PList>(context);
                Repository<Database.Models.StreamCondition> repoStreamConditions = new Repository<Database.Models.StreamCondition>(context);

                // get the stream
                Database.Models.UserDefinedObject stream = repoUserDefinedObject.Find(x => x.Name == receiveCommand.StreamName);

                // get the stream projection
                IQueryable projection = repoProjection.Filter(x => x.StreamId == stream.Id).Cast<Database.Models.PList>().OrderBy(x => x.Order);
                projectionScript = this.GenerateProjectionScript(projection);

                // get the stream conditions
                IQueryable conditions = repoStreamConditions.Filter(x => x.StreamId == stream.Id);

                foreach (Database.Models.StreamCondition condition in conditions)
                {
                    if (condition.IsOnCondition)
                    {
                        onCondition = condition.Expression;
                    }
                    else
                    {
                        whereCondition = condition.Expression;
                    }
                }
            }

            // load object
            if (onCondition.Equals(string.Empty))
            {
                this.LoadObject("httpSource", whereCondition, projectionScript, receiveCommand.Callback);
            }
            else
            {
                this.LoadObject("httpSource", "httpSource", onCondition, whereCondition, projectionScript, receiveCommand.Callback);
            }
        }

        /// <summary>
        /// Contains load stream logic.
        /// </summary>
        /// <param name="sourceFromName">Source name of the 'from' statement</param>
        /// <param name="sourceOnName">Source name of the 'on' statement</param>
        /// <param name="onCondition">On condition expression</param>
        /// <param name="whereCondition">Where condition expression</param>
        /// <param name="projection">Projection expression</param>
        /// <param name="callback">Channel for callback operations</param>
        /// <returns>Projection list</returns>
        private void LoadObject(string sourceFromName, string sourceOnName, string onCondition, string whereCondition, string projection, System.ServiceModel.OperationContext callback)
        {
            ProjectionParser projectionParser = new ProjectionParser(projection);
            PlanNode projectionNode = projectionParser.Parse();

            ExpressionParser onConditionParser = new ExpressionParser(onCondition);
            PlanNode onConditionNode = onConditionParser.Parse();

            ExpressionParser whereConditionParser = new ExpressionParser(whereCondition);
            PlanNode whereConditionNode = whereConditionParser.Parse();

            ExpressionConstructor constructor = new ExpressionConstructor();
            Func<EventObject, EventObject, List<Tuple<string, object>>> select = constructor.CompileJoinSelect(projectionNode);
            Func<EventObject, EventObject, bool> on = constructor.CompileJoinWhere(onConditionNode);
            Func<EventObject, EventObject, bool> where = constructor.CompileJoinWhere(whereConditionNode);

            ActionBlock<Tuple<EventObject, EventObject>[]> serializarYEnviar = new ActionBlock<Tuple<EventObject, EventObject>[]>(
               x =>
               {
                   SingleElementOrderablePartitioner<Tuple<EventObject, EventObject>> myOP2 = new SingleElementOrderablePartitioner<Tuple<EventObject, EventObject>>(x);
                   bool mismatch = false;
                   Parallel.ForEach(
                       myOP2,
                       (item, state, index) =>
                       {
                           if (int.Parse(item.Item1.Message[1][1].Value.ToString()) != index + 1)
                           {
                               mismatch = true;
                           }

                           if (++this.contador % 1000 == 0)
                           {
                               Console.ForegroundColor = ConsoleColor.Blue;
                               Console.WriteLine("Salen: {0}", contador);
                               Console.ResetColor();
                           }

                           byte[] result;
                           System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                           using (MemoryStream ms = new MemoryStream())
                           {
                               bf.Serialize(ms, select(item.Item1, item.Item2).ToArray());
                               result = ms.ToArray();
                           }

                           callback.GetCallbackChannel<ICommandResultCallback>().GetProjection(new CallbackResult(result));
                           Interlocked.Increment(ref counter);
                       });

                   if (mismatch)
                   {
                       Console.ForegroundColor = ConsoleColor.Red;
                       Console.WriteLine("OrderablePartitioner Test: index mismatch detected");
                       Console.ResetColor();
                   }

                   // Console.ResetColor();
                   // Console.WriteLine("OrderablePartitioner test: counter = {0}, should be 500", counter);
               },
            new ExecutionDataflowBlockOptions() { BoundedCapacity = ExecutionDataflowBlockOptions.Unbounded });

            JoinBlock<EventObject, EventObject> joinBlock = new JoinBlock<EventObject, EventObject>(new GroupingDataflowBlockOptions() { BoundedCapacity = GroupingDataflowBlockOptions.Unbounded });
            Sources.GetSource(sourceFromName).LinkTo<EventObject>(joinBlock.Target1);
            Sources.GetSource(sourceOnName).LinkTo<EventObject>(joinBlock.Target2);

            BatchBlock<Tuple<EventObject, EventObject>> batchedJoinBlock = new BatchBlock<Tuple<EventObject, EventObject>>(500, new GroupingDataflowBlockOptions() { BoundedCapacity = GroupingDataflowBlockOptions.Unbounded });

            joinBlock.LinkTo(
                batchedJoinBlock, 
                x => 
                {
                    if (on(x.Item1, x.Item2))
                    {
                        if(where(x.Item1, x.Item2))
                        {
                            return true;
                        }
                    }

                    return false;
                });

            batchedJoinBlock.LinkTo(serializarYEnviar);
        }

        /// <summary>
        /// Contains load stream logic.
        /// </summary>
        /// <param name="sourceName">Source name</param>
        /// <param name="whereCondition">Where condition expression</param>
        /// <param name="projection">Projection expression</param>
        /// <param name="callback">Channel for callback operations</param>
        private void LoadObject(string sourceName, string whereCondition, string projection, System.ServiceModel.OperationContext callback)
        {
            ProjectionParser projectionParser = new ProjectionParser(projection);
            PlanNode projectionNode = projectionParser.Parse();

            ExpressionParser whereConditionParser = new ExpressionParser(whereCondition);
            PlanNode whereConditionNode = whereConditionParser.Parse();

            ExpressionConstructor constructor = new ExpressionConstructor();
            Func<EventObject, List<Tuple<string, object>>> select = constructor.CompileSelect(projectionNode);
            Func<EventObject, bool> where = constructor.CompileWhere(whereConditionNode);

            ActionBlock<EventObject[]> serializarYEnviar = new ActionBlock<EventObject[]>(
               x =>
               {
                   SingleElementOrderablePartitioner<EventObject> myOP2 = new SingleElementOrderablePartitioner<EventObject>(x);
                   bool mismatch = false;
                   Parallel.ForEach(
                       myOP2,
                       (item, state, index) =>
                       {
                           if (int.Parse(item.Message[1][1].Value.ToString()) != index + 1)
                           {
                               mismatch = true;
                           }

                           if (++this.contador % 1000 == 0)
                           {
                               Console.ForegroundColor = ConsoleColor.Blue;
                               Console.WriteLine("Salen: {0}", contador);
                               Console.ResetColor();
                           }

                           byte[] result;
                           System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                           using (MemoryStream ms = new MemoryStream())
                           {
                               bf.Serialize(ms, select(item).ToArray());
                               result = ms.ToArray();
                           }

                           callback.GetCallbackChannel<ICommandResultCallback>().GetProjection(new CallbackResult(result));
                           Interlocked.Increment(ref counter);
                       });

                   if (mismatch)
                   {
                       Console.ForegroundColor = ConsoleColor.Red;
                       Console.WriteLine("OrderablePartitioner Test: index mismatch detected");
                       Console.ResetColor();
                   }

                   // Console.ResetColor();
                   // Console.WriteLine("OrderablePartitioner test: counter = {0}, should be 500", counter);
               },
               new ExecutionDataflowBlockOptions() { BoundedCapacity = ExecutionDataflowBlockOptions.Unbounded });

            BatchBlock<EventObject> batchBlock = new BatchBlock<EventObject>(500, new GroupingDataflowBlockOptions() { BoundedCapacity = GroupingDataflowBlockOptions.Unbounded });

            Sources.GetSource(sourceName).LinkTo<EventObject>(batchBlock, t => where(t));
            batchBlock.LinkTo(serializarYEnviar);
        }

        /// <summary>
        /// Generate the projection script of the stream
        /// </summary>
        /// <param name="projection">Projection arguments</param>
        /// <returns>Projection script</returns>
        private string GenerateProjectionScript(IQueryable projection)
        {
            string script = "select ";
            bool isFirst = true;

            foreach (Database.Models.PList tupla in projection)
            {
                if (isFirst)
                {
                    script += tupla.Expression + " as " + tupla.Alias;
                    isFirst = false;
                }
                else
                {
                    script += ", " + tupla.Expression + " as " + tupla.Alias;
                }
            }

            return script;
        }

        /// <summary>
        /// Timer callback
        /// </summary>
        /// <param name="state">State of the timer</param>
        public static void timerCallback(object state)
        {
            Console.WriteLine(Interlocked.Exchange(ref counter, 0));
        }
    }
}
