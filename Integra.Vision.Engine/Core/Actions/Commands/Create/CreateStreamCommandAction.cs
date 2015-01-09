//-----------------------------------------------------------------------
// <copyright file="CreateStreamCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of create a new stream.
    /// </summary>
    internal sealed class CreateStreamCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Create adapter command
        /// </summary>
        private CreateStreamCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as CreateStreamCommand;

            try
            {
                if (this.command.IsSimpleStream)
                {
                    this.SaveSimpleStreamArguments();
                }
                else
                {
                    this.SaveJoinStreamArguments();
                }

                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// save the simple stream command arguments
        /// </summary>
        private void SaveSimpleStreamArguments()
        {
            // Initialize the context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.PList> repoPList = new Database.Repositories.Repository<Database.Models.PList>(vc);
            Database.Repositories.Repository<Database.Models.StreamCondition> repoStreamCondition = new Database.Repositories.Repository<Database.Models.StreamCondition>(vc);
            Database.Repositories.Repository<Database.Models.SourceAssignedToStream> repoAsig = new Database.Repositories.Repository<Database.Models.SourceAssignedToStream>(vc);
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);

            Database.Models.Stream stream = new Database.Models.Stream() { Name = this.command.Name, Type = ObjectTypeEnum.Stream.ToString(), IsSystemObject = false, State = (int)UserDefinedObjectStateEnum.Stopped, CreationDate = DateTime.Now, UseJoin = false };
            repoStream.Create(stream);

            string sourceName = this.command.From;
            Database.Models.Source source = repoSource.Find(x => x.Name == sourceName);
            Database.Models.SourceAssignedToStream asig = new Database.Models.SourceAssignedToStream() { Alias = this.command.From, IsWithSource = false, StreamId = stream.Id, SourceId = source.Id };
            repoAsig.Create(asig);

            Database.Models.StreamCondition streamCondition = new Database.Models.StreamCondition() { StreamId = stream.Id, Expression = this.command.Where, IsOnCondition = false, Type = (int)ConditionTypeEnum.FilterCondition };
            repoStreamCondition.Create(streamCondition);

            int position = 0;
            foreach (Tuple<string, string> a in this.command.Select)
            {
                Database.Models.PList projectionTuple = new Database.Models.PList() { Alias = a.Item1, StreamId = stream.Id, Expression = a.Item2, Order = position };
                repoPList.Create(projectionTuple);
                position++;
            }

            // save dependencies of the stream
            DependencyActions dependencyAction = new DependencyActions(vc, this.command.Dependencies);
            dependencyAction.SaveDependencies(stream);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }

        /// <summary>
        /// save the join stream command arguments
        /// </summary>
        private void SaveJoinStreamArguments()
        {
            // Initialize the context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.PList> repoPList = new Database.Repositories.Repository<Database.Models.PList>(vc);
            Database.Repositories.Repository<Database.Models.StreamCondition> repoStreamCondition = new Database.Repositories.Repository<Database.Models.StreamCondition>(vc);
            Database.Repositories.Repository<Database.Models.SourceAssignedToStream> repoAsig = new Database.Repositories.Repository<Database.Models.SourceAssignedToStream>(vc);
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);

            TimeSpan d;
            TimeSpan.TryParse(this.command.ApplyWindow, out d);

            Database.Models.Stream stream = new Database.Models.Stream() { Name = this.command.Name, Type = this.command.Type.ToString(), IsSystemObject = false, State = (int)UserDefinedObjectStateEnum.Stopped, CreationDate = DateTime.Now, UseJoin = true, DurationTime = d.TotalMilliseconds };
            repoStream.Create(stream);

            Database.Models.StreamCondition streamOnCondition = new Database.Models.StreamCondition() { StreamId = stream.Id, Expression = this.command.On, IsOnCondition = true, Type = (int)ConditionTypeEnum.CombinationCondition };
            repoStreamCondition.Create(streamOnCondition);

            string sourceName = this.command.JoinSourceName;
            Database.Models.Source source = repoSource.Find(x => x.Name == this.command.JoinSourceName);
            Database.Models.SourceAssignedToStream asig = new Database.Models.SourceAssignedToStream() { Alias = this.command.JoinSourceAlias, IsWithSource = false, StreamId = stream.Id, SourceId = source.Id };
            repoAsig.Create(asig);

            source = repoSource.Find(x => x.Name == this.command.WithSourceName);
            asig = new Database.Models.SourceAssignedToStream() { Alias = this.command.WithSourceAlias, IsWithSource = true, StreamId = stream.Id, SourceId = source.Id };
            repoAsig.Create(asig);

            Database.Models.StreamCondition streamCondition = new Database.Models.StreamCondition() { StreamId = stream.Id, Expression = this.command.Where, IsOnCondition = false, Type = (int)ConditionTypeEnum.MergeResultCondition };
            repoStreamCondition.Create(streamCondition);

            int position = 0;
            foreach (var a in this.command.Select)
            {
                Database.Models.PList projectionTuple = new Database.Models.PList() { Alias = a.Item1, StreamId = stream.Id, Expression = a.Item2, Order = position };
                repoPList.Create(projectionTuple);
                position++;
            }

            // save dependencies of the stream
            DependencyActions dependencyAction = new DependencyActions(vc, this.command.Dependencies);
            dependencyAction.SaveDependencies(stream);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
