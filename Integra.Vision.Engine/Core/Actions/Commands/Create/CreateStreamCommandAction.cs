//-----------------------------------------------------------------------
// <copyright file="CreateStreamCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of create a new stream.
    /// </summary>
    internal sealed class CreateStreamCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            CreateStreamCommand createStreamCommand = command as CreateStreamCommand;
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    if (createStreamCommand.IsSimpleStream)
                    {
                        this.SaveSimpleStreamArguments(context, createStreamCommand);
                    }
                    else
                    {
                        this.SaveJoinStreamArguments(context, createStreamCommand);
                    }

                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Save the simple stream command arguments
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Create stream command</param>
        private void SaveSimpleStreamArguments(ObjectsContext vc, CreateStreamCommand command)
        {
            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.PList> repoPList = new Database.Repositories.Repository<Database.Models.PList>(vc);
            Database.Repositories.Repository<Database.Models.StreamCondition> repoStreamCondition = new Database.Repositories.Repository<Database.Models.StreamCondition>(vc);
            Database.Repositories.Repository<Database.Models.SourceAssignedToStream> repoAsig = new Database.Repositories.Repository<Database.Models.SourceAssignedToStream>(vc);
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);

            Database.Models.Stream stream = new Database.Models.Stream() { Name = command.Name, Type = ObjectTypeEnum.Stream.ToString(), IsSystemObject = false, State = (int)UserDefinedObjectStateEnum.Stopped, CreationDate = DateTime.Now, UseJoin = false };
            repoStream.Create(stream);

            string sourceName = command.From;
            Database.Models.Source source = repoSource.Find(x => x.Name == sourceName);
            Database.Models.SourceAssignedToStream asig = new Database.Models.SourceAssignedToStream() { Alias = command.From, IsWithSource = false, StreamId = stream.Id, SourceId = source.Id };
            repoAsig.Create(asig);

            Database.Models.StreamCondition streamCondition = new Database.Models.StreamCondition() { StreamId = stream.Id, Expression = command.Where, IsOnCondition = false, Type = (int)ConditionTypeEnum.FilterCondition };
            repoStreamCondition.Create(streamCondition);

            int position = 0;
            foreach (Tuple<string, string> a in command.Select)
            {
                Database.Models.PList projectionTuple = new Database.Models.PList() { Alias = a.Item1, StreamId = stream.Id, Expression = a.Item2, Order = position };
                repoPList.Create(projectionTuple);
                position++;
            }

            // save the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.SaveScript(command.Script, stream.Id);

            // save dependencies of the stream
            DependencyActions dependencyAction = new DependencyActions(vc, command.Dependencies);
            dependencyAction.SaveDependencies(stream);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }

        /// <summary>
        /// save the join stream command arguments
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Create stream command</param>
        private void SaveJoinStreamArguments(ObjectsContext vc, CreateStreamCommand command)
        {
            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.PList> repoPList = new Database.Repositories.Repository<Database.Models.PList>(vc);
            Database.Repositories.Repository<Database.Models.StreamCondition> repoStreamCondition = new Database.Repositories.Repository<Database.Models.StreamCondition>(vc);
            Database.Repositories.Repository<Database.Models.SourceAssignedToStream> repoAsig = new Database.Repositories.Repository<Database.Models.SourceAssignedToStream>(vc);
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);

            TimeSpan d;
            TimeSpan.TryParse(command.ApplyWindow, out d);

            Database.Models.Stream stream = new Database.Models.Stream() { Name = command.Name, Type = command.Type.ToString(), IsSystemObject = false, State = (int)UserDefinedObjectStateEnum.Stopped, CreationDate = DateTime.Now, UseJoin = true, DurationTime = d.TotalMilliseconds };
            repoStream.Create(stream);

            Database.Models.StreamCondition streamOnCondition = new Database.Models.StreamCondition() { StreamId = stream.Id, Expression = command.On, IsOnCondition = true, Type = (int)ConditionTypeEnum.CombinationCondition };
            repoStreamCondition.Create(streamOnCondition);

            string sourceName = command.JoinSourceName;
            Database.Models.Source source = repoSource.Find(x => x.Name == command.JoinSourceName);
            Database.Models.SourceAssignedToStream asig = new Database.Models.SourceAssignedToStream() { Alias = command.JoinSourceAlias, IsWithSource = false, StreamId = stream.Id, SourceId = source.Id };
            repoAsig.Create(asig);

            source = repoSource.Find(x => x.Name == command.WithSourceName);
            asig = new Database.Models.SourceAssignedToStream() { Alias = command.WithSourceAlias, IsWithSource = true, StreamId = stream.Id, SourceId = source.Id };
            repoAsig.Create(asig);

            Database.Models.StreamCondition streamCondition = new Database.Models.StreamCondition() { StreamId = stream.Id, Expression = command.Where, IsOnCondition = false, Type = (int)ConditionTypeEnum.MergeResultCondition };
            repoStreamCondition.Create(streamCondition);

            int position = 0;
            foreach (var a in command.Select)
            {
                Database.Models.PList projectionTuple = new Database.Models.PList() { Alias = a.Item1, StreamId = stream.Id, Expression = a.Item2, Order = position };
                repoPList.Create(projectionTuple);
                position++;
            }

            // save the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.SaveScript(command.Script, stream.Id);

            // save dependencies of the stream
            DependencyActions dependencyAction = new DependencyActions(vc, command.Dependencies);
            dependencyAction.SaveDependencies(stream);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
