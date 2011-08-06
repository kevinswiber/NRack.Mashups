using System;
using System.IO;
using Jurassic.Library;
using NRack.Configuration;

namespace Mixup.Web
{
    public class Config : ConfigBase
    {
        public override void Start()
        {
            JurassicBootstrapper.Initialize();

            var engine = JurassicBootstrapper.GetScriptEngine();

            engine.SetGlobalFunction("map", ((Action<string, ObjectInstance>)((str, obj) => Map(str, ConvertObjectInstanceToCallable(obj)))));
            engine.SetGlobalFunction("run", (Action<ObjectInstance>)(obj => Run(new JavaScriptApp(obj))));

            // Map JavaScript functions to C# functions

            //conf.CallMemberFunction("config");
            //Map("/",
            //    config => config.Run(new JavaScriptApp(@"app\hello_world")))
            //.Map("/echo",
            //    config => config.Run(new JavaScriptApp(@"app\echo")));

            new JavaScriptEvaluator().Evaluate("config");
        }

        private dynamic ConvertObjectInstanceToCallable(ObjectInstance obj)
        {
            return new JavaScriptApp(obj);
        }
    }
}