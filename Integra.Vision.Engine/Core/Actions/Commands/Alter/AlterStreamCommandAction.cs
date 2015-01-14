//-----------------------------------------------------------------------
// <copyright file="AlterStreamCommandAction.cs" company="Integra.Vision.Engine">
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
    internal sealed class AlterStreamCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                AlterStreamCommand alterStreamCommand = command as AlterStreamCommand;

                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    if (alterStreamCommand.IsSimpleStream)
                    {
                        this.UpdateSimpleStreamArguments(context, alterStreamCommand);
                    }
                    else
                    {
                        this.UpdateJoinStreamArguments(context, alterStreamCommand);
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
        /// Contains alter stream logic.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Alter stream command</param>
        private void UpdateSimpleStreamArguments(ViewsContext vc, AlterStreamCommand command)
        {
            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.PList> repoPList = new Database.Repositories.Repository<Database.Models.PList>(vc);
            Database.Repositories.Repository<Database.Models.StreamCondition> repoStreamCondition = new Database.Repositories.Repository<Database.Models.StreamCondition>(vc);
            Database.Repositories.Repository<Database.Models.SourceAssignedToStream> repoAsig = new Database.Repositories.Repository<Database.Models.SourceAssignedToStream>(vc);
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            // get the stream
            Database.Models.Stream stream = repoStream.Find(x => x.Name == command.Name);

            // detele the conditions
            repoStreamCondition.Delete(x => x.StreamId == stream.Id);

            // delete the projection
            repoPList.Delete(x => x.StreamId == stream.Id);

            // delete asignations sources-stream
            repoAsig.Delete(x => x.StreamId == stream.Id);

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == stream.Id);

            // update the stream arguments
            stream.Type = ObjectTypeEnum.Stream.ToString();
            stream.IsSystemObject = false;
            stream.State = (int)UserDefinedObjectStateEnum.Stopped;
            stream.CreationDate = DateTime.Now;
            stream.UseJoin = false;

            // update the stream
            repoStream.Update(stream);

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

            // update the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.UpdateScript(command.Script, stream.Id);

            // save dependencies of the stream
            DependencyActions dependencyAction = new DependencyActions(vc, command.Dependencies);
            dependencyAction.SaveDependencies(stream);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }

        /// <summary>
        /// Contains alter stream logic.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Alter stream command</param>
        private void UpdateJoinStreamArguments(ViewsContext vc, AlterStreamCommand command)
        {
            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.PList> repoPList = new Database.Repositories.Repository<Database.Models.PList>(vc);
            Database.Repositories.Repository<Database.Models.StreamCondition> repoStreamCondition = new Database.Repositories.Repository<Database.Models.StreamCondition>(vc);
            Database.Repositories.Repository<Database.Models.SourceAssignedToStream> repoAsig = new Database.Repositories.Repository<Database.Models.SourceAssignedToStream>(vc);
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);
            Database.Repositories.Repository<Database.Models.Dependency> repoDependency = new Database.Repositories.Repository<Database.Models.Dependency>(vc);

            TimeSpan d;
            TimeSpan.TryParse(command.ApplyWindow, out d);

            // get the stream
            Database.Models.Stream stream = repoStream.Find(x => x.Name == command.Name);

            // detele the conditions
            repoStreamCondition.Delete(x => x.StreamId == stream.Id);

            // delete the projection
            repoPList.Delete(x => x.StreamId == stream.Id);

            // delete asignations sources-stream
            repoAsig.Delete(x => x.StreamId == stream.Id);

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == stream.Id);

            // update the stream arguments
            stream.Type = ObjectTypeEnum.Stream.ToString();
            stream.IsSystemObject = false;
            stream.State = (int)UserDefinedObjectStateEnum.Stopped;
            stream.CreationDate = DateTime.Now;
            stream.UseJoin = true;
            stream.DurationTime = d.TotalMilliseconds;

            // update the stream
            repoStream.Update(stream);

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

            // update the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.UpdateScript(command.Script, stream.Id);

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
