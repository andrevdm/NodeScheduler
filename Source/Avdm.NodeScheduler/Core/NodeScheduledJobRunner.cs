using System;
using Avdm.NetTp.Messaging;
using StructureMap;

namespace Avdm.Scheduler.Core
{
    public interface INodeScheduledJobRunner
    {
    }

    public class NodeScheduledJobRunner : INodeScheduledJobRunner
    {
        private readonly INodeScheduledJobListener m_listener;

        public NodeScheduledJobRunner()
        {
            m_listener = ObjectFactory.GetInstance<INodeScheduledJobListener>();
            m_listener.ScheduledJobCommandMessageRecieved += OnScheduledJobCommandMessageRecieved;
        }

        private void OnScheduledJobCommandMessageRecieved( ScheduledJobCommandMessage message )
        {
            //TODO run jobs in nodes, check if already running etc
            Console.WriteLine( "run scheduled job - {0}", message.JobToRun );
        }
    }

    public class NodeScheduledJobListener : INodeScheduledJobListener
    {
        public event Action<ScheduledJobCommandMessage> ScheduledJobCommandMessageRecieved = delegate { };

        public NodeScheduledJobListener()
        {
            var bus = ObjectFactory.GetInstance<INetTpMessageBus>();
            bus.SubscribeToCommand<ScheduledJobCommandMessage>( 
                "NodeScheduledJobListener", 
                Environment.MachineName,
                msg =>
                    {
                        ScheduledJobCommandMessageRecieved( msg );
                    } );
        }
    }

    public interface INodeScheduledJobListener
    {
        event Action<ScheduledJobCommandMessage> ScheduledJobCommandMessageRecieved;
    }

    public interface INodeScheduledJobSettingsPersister
    {
        //TODO
    }
}
