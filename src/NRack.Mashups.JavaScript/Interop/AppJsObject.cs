using System;
using System.Collections.Generic;
using IronJS;

namespace NRack.Mashups.JavaScript.Interop
{
    public class AppJsObject : CommonObject
    {
        public AppJsObject(IronJS.Environment env, ICallable app) 
            : base(env, env.NewObject())
        {
            Put("call", 
                IronJS.Native.Utils.CreateFunction<Func<BoxedValue, CommonObject>>(env, 1, 
                                                                                   environ => ConvertArrayToObject(env, app.Call(CreateDictionary(environ.Object as EnvironmentJsObject)))));
        }

        private static CommonObject ConvertArrayToObject(IronJS.Environment env, object[] objects)
        {
            var obj = env.NewObject();
            obj.Put("status", (string)objects[0]);
            obj.Put("headers", new EnvironmentJsObject((IDictionary<string, object>) objects[1], env, env.NewPrototype()));
            obj.Put("body", objects[2]);

            return obj;

        }

        private static IDictionary<string, object> CreateDictionary(EnvironmentJsObject environ)
        {
            var dict = new Dictionary<string, object>();

            foreach(var prop in environ.Members.Keys)
            {
                dict[prop] = environ.Members[prop];
            }

            return dict;
        }
    }
}