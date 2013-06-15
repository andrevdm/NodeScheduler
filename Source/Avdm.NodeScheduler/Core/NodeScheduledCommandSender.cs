using System;
using Avdm.NetTp.Messaging;
using Quartz;
using StructureMap;

namespace Avdm.Scheduler.Core
{
    public class NodeScheduledCommandSender : IJob
    {
        public void Execute( IJobExecutionContext context )
        {
            var bus = ObjectFactory.GetInstance<INetTpMessageBus>();

            var missedTriggerAction = (MissedTriggerAction)Enum.Parse( typeof( MissedTriggerAction ), context.JobDetail.JobDataMap["#missedTriggerAction"].ToString() );
            var previousJobRunningAction = (PreviousJobRunningAction)Enum.Parse( typeof( PreviousJobRunningAction ), context.JobDetail.JobDataMap["#previousJobRunningAction"].ToString() );
            var jobToRun = context.JobDetail.JobDataMap["#jobToRun"].ToString();
            var runOn = context.JobDetail.JobDataMap["#runOnMachineName"].ToString();

            bus.PublishCommand( runOn, new ScheduledJobCommandMessage( missedTriggerAction, previousJobRunningAction, jobToRun, runOn ) );
        }
    }
}