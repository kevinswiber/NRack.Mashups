using System.Collections.Generic;
using Jurassic.Library;
using NRack;

namespace Mixup
{
    public class JavaScriptApp : ICallable
    {
        private readonly ObjectInstance _objectInstance;

        public JavaScriptApp(ObjectInstance obj)
        {
            _objectInstance = obj;
        }

        public dynamic[] Call(IDictionary<string, dynamic> environment)
        {
            var response = _objectInstance.CallMemberFunction("call", 
                new Environment(_objectInstance.Engine, environment)) as ObjectInstance;

            return new JavaScriptAppResponseConverter().ConvertJavaScriptResponse(response);
        }
    }
}