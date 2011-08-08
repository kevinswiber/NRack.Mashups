using System;
using System.IO;
using IronJS;
using NRack;
using NRack.Configuration;

namespace Mixup
{
    public class Config : ConfigBase
    {
        public override void Start()
        {
            IronJsBootstrap.Initialize();

            var context = IronJsBootstrap.GetContext();

            context.SetGlobal("run", 
                IronJS.Native.Utils.CreateFunction<Action<BoxedValue>>(context.Environment, 1, obj => Run(new JavaScriptApp(obj.Object))));

            context.SetGlobal("use",
                IronJS.Native.Utils.CreateFunction<Action<CommonObject>>(context.Environment, 1,
                    obj => Use(typeof(JavaScriptApp), obj)));

            context.SetGlobal("map",
                IronJS.Native.Utils.CreateFunction<Action<string, FunctionObject>>(context.Environment, 2,
                (str, func) => Map(str, BuilderJsObject.MapBuilder(func))));

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var configFile = Path.Combine(baseDir, "config.js");

            context.Execute(IronJsBootstrap.ReadSource(configFile));
        }

        public class BuilderJsObject : CommonObject
        {
            public BuilderJsObject(IronJS.Environment env, Builder builder)
                : base(env, env.NewObject())
            {
                Put("run",
                    IronJS.Native.Utils.CreateFunction<Action<CommonObject>>(env, 1, obj => builder.Run(new JavaScriptApp(obj))));

                Put("map",
                    IronJS.Native.Utils.CreateFunction<Action<string, FunctionObject>>(env, 2,
                        (str, func) => builder.Map(str, MapBuilder(func))));
            }

            public static Action<Builder> MapBuilder(FunctionObject func)
            {
                return builder => func.Call(func.Env.NewObject(), new BuilderJsObject(func.Env, builder));
            }
        }
    }
}