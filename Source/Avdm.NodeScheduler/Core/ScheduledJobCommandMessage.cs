using Avdm.NetTp.Messaging;

namespace Avdm.Scheduler.Core
{
    public class ScheduledJobCommandMessage : NetTpCommandMessage
    {
        public ScheduledJobCommandMessage( MissedTriggerAction missedTriggerAction, PreviousJobRunningAction previousJobRunningAction, string jobToRun, string runOn )
        {
            MissedTriggerAction = missedTriggerAction;
            PreviousJobRunningAction = previousJobRunningAction;
            JobToRun = jobToRun;
            RunOn = runOn;
        }

        public MissedTriggerAction MissedTriggerAction { get; private set; }
        public PreviousJobRunningAction PreviousJobRunningAction { get; private set; }
        public string JobToRun { get; private set; }
        public string RunOn { get; private set; }
    }
}