namespace Avdm.Scheduler.Core
{
    public interface INodeTrigger
    {
        string Cron { get; }
        string Description { get; }
    }
}