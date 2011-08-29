using System;
using System.IO;
using IronJS;
using IronJS.Hosting;
using NRack.Configuration;
using NRack.Mashups.JavaScript.Interop;

namespace NRack.Mashups.JavaScript
{
    public class Config : ConfigBase
    {
        private static string _sourceFile;
        public static string SourceFile
        {
            get
            {
                if (_sourceFile == null)
                {
                    var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    _sourceFile = Path.Combine(baseDir, "config.js");
                }

                return _sourceFile;
            }
            set { _sourceFile = value;  }
        }

        public override void Start()
        {
            var context = IronJsBootstrap.Initialize();

            WireUpJavaScriptConfigurationObject(context);

            var source = GetSourceString();

            context.Execute(@"(function () {
                               var config = new Config();
                               function run (obj) { config.run(obj); }
                               function use (obj) { config.use(obj); }
                               function map (str, obj) { config.map(str, obj); }" +
                               System.Environment.NewLine + source + System.Environment.NewLine +
                               @"})();");
        }

        private static string GetSourceString()
        {
            return IronJsBootstrap.ReadSource(SourceFile);
        }

        private void WireUpJavaScriptConfigurationObject(CSharp.Context context)
        {
            var prototype = context.Environment.NewObject();
            var constructor = IronJS.Native.Utils.CreateConstructor<Func<FunctionObject, CommonObject, CommonObject>>(
                context.Environment, 0, (ctor, _) =>
                                            {
                                                var proto = ctor.GetT<CommonObject>("prototype");
                                                return new ConfigJsObject(ctor.Env, this, proto);
                                            });

            prototype.Prototype = context.Environment.Prototypes.Object;

            prototype.Put("run",
                          IronJS.Native.Utils.CreateFunction<Action<FunctionObject, CommonObject, BoxedValue>>(
                              context.Environment, 1, ConfigJsObject.Run));

            prototype.Put("use",
                          IronJS.Native.Utils.CreateFunction<Action<FunctionObject, CommonObject, CommonObject>>(
                              context.Environment, 1, ConfigJsObject.Use));

            prototype.Put("map",
                          IronJS.Native.Utils.CreateFunction<Action<FunctionObject, CommonObject, string, BoxedValue>>(
                              context.Environment, 1, ConfigJsObject.Map));

            constructor.Put("prototype", prototype);

            context.SetGlobal("Config", constructor);
        }

    }
}