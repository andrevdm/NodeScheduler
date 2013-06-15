namespace Avdm.Scheduler.Core
{
    public class NodeCronTrigger : INodeTrigger
    {
        public NodeCronTrigger( string description, string cron )
        {
            Cron = cron;
            Description = description;
        }

        public string Cron { get; private set; }
        public string Description { get; private set; }
    }
}