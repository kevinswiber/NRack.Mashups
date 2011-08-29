using System;
using System.Linq;
using System.Net;
using Gate;
using Gate.Kayak;
using Kayak;
using NRack.Hosting.Owin;
using NRack.Mashups.JavaScript;

namespace Mashup
{

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any())
            {
                Config.SourceFile = args[0];
            }

            var endPoint = new IPEndPoint(IPAddress.Any, 8080);

            Console.WriteLine("Running... {0}.", endPoint);

            KayakGate.Start(new SchedulerDelegate(), endPoint, Startup.Configuration);

            Console.ReadLine();
        }
    }

    class Startup
    {
        public static void Configuration(IAppBuilder appBuilder)
        {
            var handler = new OwinHandler();
            appBuilder
                .RescheduleCallbacks()
                .Run(Delegates.ToDelegate(handler.ProcessRequest));
        }
    }

    class SchedulerDelegate : ISchedulerDelegate
    {
        public void OnException(IScheduler scheduler, Exception e)
        {
            Console.WriteLine(e);
        }

        public void OnStop(IScheduler scheduler)
        {
            Console.WriteLine("Stopped");
        }
    }

}
