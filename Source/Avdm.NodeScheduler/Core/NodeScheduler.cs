using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Avdm.Config;
using Avdm.Core;
using Quartz;

namespace Avdm.Scheduler.Core
{
    public class NodeScheduler : INodeScheduler
    {
        private static readonly IScheduler g_scheduler;

        static NodeScheduler()
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = ConfigManager.AppSettings["Scheduler.InstanceName"];
            properties["quartz.scheduler.instanceId"] = Environment.MachineName + DateTime.UtcNow.Ticks;
            properties["quartz.jobStore.type"] = "Quartz.Impl.MongoDB.JobStore, Quartz.Impl.MongoDB";
            //TODO Quartz.Impl.MongoDB.JobStore.DefaultConnectionString = ConfigManager.AppSettings["Scheduler.ConnectionString"];

            g_scheduler = new Quartz.Impl.StdSchedulerFactory( properties ).GetScheduler();
            g_scheduler.Start();
        }

        public void AddOrReplaceCommandMessageJob(
            string jobId,
            string description,
            string jobToRun,
            string runOnMachineName,
            MissedTriggerAction missedTriggerAction,
            PreviousJobRunningAction previousJobRunningAction,
            DateTime startTime,
            IEnumerable<INodeTrigger> triggers )
        {
            Preconditions.CheckNotBlank( jobId, "jobId" );
            Preconditions.CheckNotBlank( description, "description" );
            Preconditions.CheckNotBlank( jobToRun, "jobToRun" );
            Preconditions.CheckNotBlank( runOnMachineName, "runOnMachineName" );
            Preconditions.CheckNotNull( triggers, "triggers" );
            Preconditions.CheckNotBlank( jobId, "jobId" );
            Preconditions.CheckNotBlank( jobId, "jobId" );
            Preconditions.CheckNotBlank( jobId, "jobId" );

            var job = JobBuilder.Create<NodeScheduledCommandSender>()
                                            .WithIdentity( "job_" + jobId, "defaultGroup" )
                                            .UsingJobData( "#missedTriggerAction", missedTriggerAction.ToString() )
                                            .UsingJobData( "#previousJobRunningAction", previousJobRunningAction.ToString() )
                                            .UsingJobData( "#jobToRun", jobToRun ?? "" )
                                            .UsingJobData( "#runOnMachineName", runOnMachineName ?? "" )
                                            .Build();

            var quartzTriggers = new List<ITrigger>();

            foreach( var trigger in triggers )
            {
                quartzTriggers.Add( TriggerBuilder.Create()
                                        .WithIdentity( "trigger_" + quartzTriggers.Count + "_" + trigger.Description + "_for_" + jobId, "group1" )
                                        .ForJob( job )
                                        .StartAt( startTime )
                                        .WithCronSchedule( trigger.Cron )
                                        .WithDescription( description )
                                        .Build() );
            }

            if( quartzTriggers.Count == 0 )
            {
                throw new ArgumentException( "No triggers defined", "triggers" );
            }

            g_scheduler.ScheduleJob( job, new Quartz.Collection.HashSet<ITrigger>( quartzTriggers ), true );
        }
    }
}