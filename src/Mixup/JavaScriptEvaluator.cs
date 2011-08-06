using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Jurassic;
using Jurassic.Library;

namespace Mixup
{
    public class JavaScriptEvaluator
    {
        private static readonly ScriptEngine Engine = JurassicBootstrapper.GetScriptEngine();

        public ObjectInstance Evaluate(string fileName)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            fileName = Path.Combine(baseDir, fileName);

            var module = Engine.CallGlobalFunction("require", fileName) as ObjectInstance;

            if (module == null)
            {
                throw new InvalidOperationException("File does not exist: " + fileName);
            }

            return module;
        }
    }
}
