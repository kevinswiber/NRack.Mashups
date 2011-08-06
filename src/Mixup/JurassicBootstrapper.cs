using System;
using System.IO;
using Jurassic;

namespace Mixup
{
    public class JurassicBootstrapper
    {
        static readonly ScriptEngine Engine = new ScriptEngine();
        private static readonly string CommonJS = GetCommonJS();
        private delegate string SourceRead(string fileName);

        private static bool _isInitialized;
        private static readonly object SyncLock = new object();

        public static void Initialize()
        {
            if (!_isInitialized)
            {
                lock (SyncLock)
                {
                    if (!_isInitialized)
                    {
                        Engine.Execute(CommonJS);
                        Engine.SetGlobalFunction("read", (SourceRead) ReadSource);
                        _isInitialized = true;
                    }
                }
            }
        }

        public static ScriptEngine GetScriptEngine()
        {
            return Engine;
        }

        private static string GetCommonJS()
        {
            string commonJS;

            var assembly = typeof(JavaScriptEvaluator).Assembly;
            using (var stream = assembly.GetManifestResourceStream("Mixup.common.js"))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("CommonJS file is missing!");
                }

                stream.Position = 0;
                commonJS = new StreamReader(stream).ReadToEnd();
                stream.Close();
            }

            return commonJS;
        }

        private static string ReadSource(string fileName)
        {
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            var sourceStorage = SourceStorage.Instance.Sources;
            if (!sourceStorage.ContainsKey(fileName))
            {
                using (var streamReader = new StreamReader(fileName))
                {
                    sourceStorage[fileName] = streamReader.ReadToEnd();
                }

                Engine.SetGlobalValue("sourceStorage", SourceStorage.Instance);
            }

            var source = sourceStorage[fileName];

            return source;
        }
    }
}