using System;
using Topshelf;
using System.Diagnostics;
using Nancy.Hosting.Self;

using Rhino.Geometry;

namespace Medina.Api
{
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>
            {
                x.Service<NancySelfHost>(s =>
                {
                    s.ConstructUsing(name => new NancySelfHost());

                    s.WhenStarted(tc => { tc.Start(); });
                    s.WhenStopped(tc => { tc.Stop(); });
                });

                x.RunAsLocalSystem();
                x.SetDescription("medina protocol");
                x.SetDisplayName("medina api");
                x.SetServiceName("rch.dev.medina");
            });
        }
    }

    public class NancySelfHost
    {
        private NancyHost m_nancyHost;

        public void Start()
        {
            Startup.LaunchInProcess(Startup.LoadMode.Headless, 0);
            Console.WriteLine("Rhino loaded at port 1001.");
            m_nancyHost = new NancyHost(new Uri("http://localhost:1001"));
            m_nancyHost.Start();

        }

        public void Stop()
        {
            m_nancyHost.Stop();
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}