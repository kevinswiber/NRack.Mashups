using System;
using System.Collections.Generic;
using System.IO;
using IronJS;

namespace NRack.Mixups.JavaScript.Interop
{
    public class EnvironmentJsObject : CommonObject
    {
        public EnvironmentJsObject(IDictionary<string, object> environment, IronJS.Environment env, CommonObject prototype) 
            : base(env, env.Maps.Base, prototype)
        {
            foreach(var key in environment.Keys)
            {
                if (key == "rack.input")
                {
                    Put(key, new JsStream(env, environment[key] as Stream));
                    continue;
                }

                Put(key, environment[key].ToString());
            }
        }

        class JsStream : CommonObject
        {
            public JsStream(IronJS.Environment env, Stream stream) 
                : base(env, env.NewPrototype())
            {
                Put("read",
                    IronJS.Native.Utils.CreateFunction<Func<CommonObject>>(env, 0,
                        () =>
                        {
                            if (stream == null)
                            {
                                return null;
                            }

                            string input;
                            using (var strm = stream)
                            {
                                strm.Position = 0;
                                var reader = new StreamReader(strm);
                                input = reader.ReadToEnd();
                                strm.Close();
                            }

                            return env.NewString(input);
                        }));
            }
        }
    }
}