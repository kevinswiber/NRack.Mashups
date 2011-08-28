using System;
using System.Collections.Generic;
using IronJS;
using NRack.Mashups.JavaScript.Interop;

namespace NRack.Mashups.JavaScript
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
                                                                              new AppJsObject(_objectInstance.Env, (ICallable)app));
            }
        }

        public JavaScriptApp(CommonObject obj)
            : this(null, obj)
        {
        }

        public dynamic[] Call(IDictionary<string, object> environment)
        {
            if (!_objectInstance.Members.ContainsKey("call"))
            {
                throw new InvalidOperationException("Application is missing a call export.");
            }

            var response = ((FunctionObject) _objectInstance.Members["call"])
                .Call(_objectInstance, new EnvironmentJsObject(environment, _objectInstance.Env, _objectInstance.Env.NewPrototype())).Object;
            return new JavaScriptAppResponseConverter().ConvertJavaScriptResponse(response);
        }
    }
}