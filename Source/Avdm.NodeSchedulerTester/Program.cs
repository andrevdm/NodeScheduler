using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avdm.Core.TestApp;
using Avdm.Scheduler.Core;
using StructureMap;

namespace Avdm.NodeSchedulerTester
{
    public class TesterProgram : ConsoleTestApp
    {
        static void Main( string[] args )
        {
            ObjectFactory.Configure(
                x =>
                {
                    x.For<INodeScheduler>().Use<NodeScheduler>();
                    x.For<INodeScheduledJobListener>().Singleton().Use<NodeScheduledJobListener>();
                    x.For<INodeScheduledJobRunner>().Singleton().Use<NodeScheduledJobRunner>();
                } );

            new TesterProgram().Start();
        }

        public TesterProgram()
        {
            MenuItems.Add( new MenuItem( "Scheduler", "Demo", SchedulerDemo ) );
        }

        private void SchedulerDemo()
        {
            var jobRunner = ObjectFactory.GetInstance<INodeScheduledJobRunner>();

            var scheduler = ObjectFactory.GetInstance<INodeScheduler>();
            scheduler.AddOrReplaceCommandMessageJob(
                "testJob",
                "testing",
                "someJob",
                Environment.MachineName,
                MissedTriggerAction.Skip,
                PreviousJobRunningAction.RunInParallel,
                DateTime.Now,
                new INodeTrigger[]
                {
                    new NodeCronTrigger( "5 sec", "0/5 * * * * ?" ), 
                } );
        }

    }
}
