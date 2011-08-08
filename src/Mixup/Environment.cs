using System.Collections.Generic;
using IronJS;

namespace Mixup
{
    public class Environment : CommonObject
    {
        public Environment(IDictionary<string, object> environment, IronJS.Environment env, CommonObject prototype) 
            : base(env, env.Maps.Base, prototype)
        {
            foreach(var key in environment.Keys)
            {
                Put(key, environment[key].ToString());
            }
        }
    }
}