using System;
using System.Collections.Generic;

namespace Avdm.Scheduler.Core
{
    public interface INodeScheduler
    {
        void AddOrReplaceCommandMessageJob(
            string jobId,
            string description,
            string jobToRun,
            string runOnMachineName,
            MissedTriggerAction missedTriggerAction,
            PreviousJobRunningAction previousJobRunningAction,
            DateTime startTime,
            IEnumerable<INodeTrigger> triggers );
    }
}
