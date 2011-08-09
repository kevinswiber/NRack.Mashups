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
            var context = IronJsBootstrap.Initialize();

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var configFile = Path.Combine(baseDir, "config.js");
            var source = IronJsBootstrap.ReadSource(configFile);

            //var constructor =
            //    IronJS.Native.Utils.CreateConstructor<Func<CommonObject>>(
            //        context.Environment, 0,
            //        () => new ConfigJsObject(context.Environment, this, context.Environment.NewPrototype()));

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

            constructor.Put("prototype", prototype);

            context.SetGlobal("Config", constructor);

            context.Execute("var config = new Config();" +
                            "var ConfigRunner = function() { }; " +
                            "ConfigRunner.run = function(obj) { config.run(obj); }; " +
                            "ConfigRunner.use = function(obj) { config.use(obj); };" +
                            "ConfigRunner.map = function(str, obj) { config.map(str, obj); };" +
                            "ConfigRunner.load = function () { " + source + "\n};" +
                            "ConfigRunner.load();");
        }

        public class ConfigJsObject : CommonObject
        {
            ConfigBase configBase;

            public ConfigJsObject(IronJS.Environment env, ConfigBase config, CommonObject prototype) 
                : base(env, prototype)
            {
                //Put("run",
                //    IronJS.Native.Utils.CreateFunction<Action<BoxedValue>>(env, 1, obj => config.Run(new JavaScriptApp(obj.Object))));

                //Put("use",
                //    IronJS.Native.Utils.CreateFunction<Action<CommonObject>>(env, 1,
                //        obj => config.Use(typeof(JavaScriptApp), obj)));

                //Put("map",
                //    IronJS.Native.Utils.CreateFunction<Action<string, FunctionObject>>(env, 2,
                //        (str, func) => config.Map(str, BuilderJsObject.MapBuilder(func))));

                configBase = config;
            }

            public static void Run(FunctionObject _, CommonObject that, BoxedValue obj)
            {
                var configObj = that.CastTo<ConfigJsObject>();
                configObj.configBase.Run(new JavaScriptApp(obj.Object));
            }

            public static void Use(FunctionObject _, CommonObject that, CommonObject obj)
            {
                var configObj = that.CastTo<ConfigJsObject>();
                configObj.configBase.Use(typeof (JavaScriptApp), obj);
            }

            public static void Map(FunctionObject _, CommonObject that, string str, FunctionObject func)
            {
                var configObj = that.CastTo<ConfigJsObject>();
                configObj.configBase.Map(str, BuilderJsObject.MapBuilder(func));
            }

            public override string ClassName
            {
                get { return "Config"; }
            }

        }

        public class BuilderJsObject : CommonObject
        {
            public BuilderJsObject(IronJS.Environment env, Builder builder)
                : base(env, env.NewPrototype())
            {
                Put("run",
                    IronJS.Native.Utils.CreateFunction<Action<CommonObject>>(env, 1, obj => builder.Run(new JavaScriptApp(obj))));

                Put("use",
                    IronJS.Native.Utils.CreateFunction<Action<CommonObject>>(env, 1,
                        obj => builder.Use(typeof(JavaScriptApp), obj)));

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