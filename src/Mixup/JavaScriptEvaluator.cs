using System.Collections.Generic;
using Jurassic;
using Jurassic.Library;

namespace Mixup
{
    public class JavaScriptEvaluator
    {
        public ObjectInstance Evaluate(string source, IDictionary<string, dynamic> environment)
        {
            var engine = new ScriptEngine();
            engine.Evaluate(source);
            return engine.CallGlobalFunction<ObjectInstance>("call", new Environment(engine, environment));
        }
    }

    public class Environment : ObjectInstance
    {
        public Environment(ScriptEngine engine, IDictionary<string, dynamic> environment)
            : base(engine)
        {
            foreach(var key in environment.Keys)
            {
                base.DefineProperty(key, new PropertyDescriptor(environment[key], PropertyAttributes.FullAccess), true);
            }
        }
    }
}
