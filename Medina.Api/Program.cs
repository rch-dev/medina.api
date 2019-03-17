using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Extensions;
using Nancy.Hosting.Self;
using Nancy.Routing;
using Nancy.TinyIoc;
using Rh = Rhino;
using Topshelf;

namespace Medina.Api
{
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>
            {
                x.ApplyCommandLine();
                x.SetStartTimeout(new TimeSpan(0, 1, 0));

                x.Service<NancySelfHost>(s =>
                {
                    s.ConstructUsing(name => new NancySelfHost());

                    s.WhenStarted(tc => { tc.Start(); });
                    s.WhenStopped(tc => { tc.Stop(); });
                });

                x.RunAsPrompt();
                x.SetDescription("medina protocol");
                x.SetDisplayName("medina api");
                x.SetServiceName("medina protocol");
            });

            Startup.ExitInProcess();
        }
    }

    public class NancySelfHost
    {
        private NancyHost m_nancyHost;
        private System.Diagnostics.Process _backendProcess = null;

        public void Start()
        {
            Startup.LaunchInProcess(Startup.LoadMode.Headless, 0);
                        
            m_nancyHost = new NancyHost(new Uri("http://localhost:1001"));
            Console.WriteLine("Rhino loaded at port 1001.");

            //var plugin = Rh.RhinoApp.GetPlugInObject("Grasshopper");
            //Console.WriteLine(plugin.ToString());
            m_nancyHost.Start();

        }

        public void Stop()
        {
            m_nancyHost.Stop();
            if (_backendProcess != null)
                _backendProcess.Kill();
            Console.WriteLine("Stopped. Good bye!");
        }
    }

    public class Bootstrapper : Nancy.DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            //Log.Debug("ApplicationStartup");
            Console.WriteLine("Loading grasshopper...");

            // Load GH at startup so it can get initialized on the main thread
            var pluginObject = Rh.RhinoApp.GetPlugInObject("Grasshopper");
            var runheadless = pluginObject?.GetType().GetMethod("RunHeadless");
            if (runheadless != null)
                runheadless.Invoke(pluginObject, null);

            //Do the same for python
            var script = Rh.Runtime.PythonScript.Create();

            Nancy.StaticConfiguration.DisableErrorTraces = false;
            //pipelines.OnError += (ctx, ex) => LogError(ctx, ex);
            base.ApplicationStartup(container, pipelines);
        }
    }

    namespace Rhino
    {
        static class Python
        {
            static string _previousScript = "";
            static Rh.Runtime.PythonCompiledCode _previousCompile = null;

            public static Rh.Collections.ArchivableDictionary Evaluate(string script,
                Rh.Collections.ArchivableDictionary input,
                string[] outputNames)
            {
                var py = Rh.Runtime.PythonScript.Create();
                foreach (var kv in input)
                    py.SetVariable(kv.Key, kv.Value);
                if (!script.Equals(_previousScript))
                {
                    // Don't allow certain words in the script to attempt to avoid
                    // malicious attacks
                    string[] badwords = { "exec", "Assembly", "GetType", "Activator", "GetMethod", "GetPropert" };
                    foreach (var word in badwords)
                    {
                        if (script.IndexOf(word) >= 0)
                            throw new Exception($"Script is not allowed to contain the word {word}");
                    }

                    // validate that only Rhino namespaces are imported
                    const string import = "import ";
                    int importIndex = script.IndexOf(import);
                    while (importIndex >= 0)
                    {
                        importIndex += import.Length;
                        while (importIndex < script.Length)
                        {
                            char c = script[importIndex];
                            if (c == ' ')
                            {
                                importIndex++;
                                continue;
                            }
                            break;
                        }
                        if (script.IndexOf("Rhino", importIndex) != importIndex && script.IndexOf("rhinoscript", importIndex) != importIndex)
                            throw new Exception("Attempt to import module that is not permitted");

                        int commaAndContinuationIndex = script.IndexOfAny(new char[] { ',', '\\' }, importIndex);
                        if (commaAndContinuationIndex > 0)
                        {
                            int newlineIndex = script.IndexOf('\n', importIndex);
                            if (commaAndContinuationIndex < newlineIndex)
                                throw new Exception("Do not import multiple packages with a single import statement");
                        }

                        importIndex = script.IndexOf(import, importIndex);
                    }
                    _previousCompile = py.Compile(script);
                    _previousScript = script;
                }
                _previousCompile.Execute(py);

                var rc = new Rh.Collections.ArchivableDictionary();
                foreach (var name in outputNames)
                    rc[name] = py.GetVariable(name);
                return rc;
            }
        }
    }
}