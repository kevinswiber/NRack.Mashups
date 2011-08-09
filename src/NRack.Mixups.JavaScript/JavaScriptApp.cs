using System;
using System.Collections.Generic;
using IronJS;
using NRack;

namespace Mixup
{
    public class JavaScriptApp : ICallable
    {
        private readonly CommonObject _objectInstance;

        public JavaScriptApp(object app, CommonObject obj)
        {
            _objectInstance = obj;
            if (_objectInstance.Members.ContainsKey("initialize"))
            {
                ((FunctionObject) _objectInstance.Members["initialize"]).Call(_objectInstance,
                                                                              new JsApp(_objectInstance.Env, (ICallable)app));
            }
        }

        public JavaScriptApp(CommonObject obj)
            : this(null, obj)
        {
        }

        public dynamic[] Call(IDictionary<string, dynamic> environment)
        {
            if (!_objectInstance.Members.ContainsKey("call"))
            {
                throw new InvalidOperationException("Application is missing a call export.");
            }

            var response = ((FunctionObject) _objectInstance.Members["call"])
                .Call(_objectInstance, new Environment(environment, _objectInstance.Env, null)).Object;
            return new JavaScriptAppResponseConverter().ConvertJavaScriptResponse(response);
        }
    }

    internal class JsApp : CommonObject
    {
        public JsApp(IronJS.Environment env, ICallable app) 
            : base(env, env.NewObject())
        {
            Put("call", 
                IronJS.Native.Utils.CreateFunction<Func<BoxedValue, CommonObject>>(env, 1, 
                environ => ConvertArrayToObject(env, app.Call(CreateDictionary(environ.Object as Environment)))));
        }

        private CommonObject ConvertArrayToObject(IronJS.Environment env, object[] objects)
        {
            var obj = env.NewObject();
            obj.Put("status", (string)objects[0]);
            obj.Put("headers", new Environment((IDictionary<string, object>) objects[1], env, env.NewPrototype()));
            obj.Put("body", objects[2]);

            return obj;

        }

        private IDictionary<string, dynamic> CreateDictionary(Environment environ)
        {
            var dict = new Dictionary<string, dynamic>();

            foreach(var prop in environ.Members.Keys)
            {
                dict[prop] = environ.Members[prop];
            }

            return dict;
        }
    }
}