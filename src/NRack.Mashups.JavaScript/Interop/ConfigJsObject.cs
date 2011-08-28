using System;
using IronJS;
using NRack.Configuration;

namespace NRack.Mashups.JavaScript.Interop
{
    public class ConfigJsObject : CommonObject
    {
        private readonly ConfigBase configBase;

        public ConfigJsObject(IronJS.Environment env, ConfigBase config, CommonObject prototype)
            : base(env, prototype)
        {
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
            configObj.configBase.Use(typeof(JavaScriptApp), obj);
        }

        public static void Map(FunctionObject _, CommonObject that, string str, BoxedValue func)
        {
            var configObj = that.CastTo<ConfigJsObject>();
            configObj.configBase.Map(str, BuilderJsObject.MapBuilder(func.Func));
        }

        public override string ClassName
        {
            get { return "Config"; }
        }

        class BuilderJsObject : CommonObject
        {
            private BuilderJsObject(IronJS.Environment env, Builder builder)
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