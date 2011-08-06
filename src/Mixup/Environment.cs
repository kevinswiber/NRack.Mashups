using System.Collections.Generic;
using Jurassic;
using Jurassic.Library;

namespace Mixup
{
    public class Environment : ObjectInstance
    {
        public Environment(ScriptEngine engine, IDictionary<string, object> environment)
            : base(engine)
        {
            foreach(var key in environment.Keys)
            {
                base.DefineProperty(key, new PropertyDescriptor(environment[key], PropertyAttributes.FullAccess), true);
            }
        }
    }
}