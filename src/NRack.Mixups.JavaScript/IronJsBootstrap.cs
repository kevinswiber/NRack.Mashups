using System;
using System.IO;
using IronJS.Hosting;

namespace NRack.Mixups.JavaScript
{
    public class IronJsBootstrap
    {
        static readonly CSharp.Context Context = new CSharp.Context();
        private static readonly string CommonJS = GetCommonJS();

        private static bool _isInitialized;
        private static readonly object SyncLock = new object();

        public static CSharp.Context Initialize()
        {
            if (!_isInitialized)
            {
                lock (SyncLock)
                {
                    if (!_isInitialized)
                    {
                        Context.Execute(CommonJS);
                        Context.SetGlobal("read", 
                            IronJS.Native.Utils.CreateFunction<Func<string, string>>(Context.Environment, 1, ReadSource));

                        _isInitialized = true;
                    }
                }
            }

            return Context;
        }

        private static string GetCommonJS()
        {
            string commonJS;

            var assembly = typeof(IronJsBootstrap).Assembly;
            using (var stream = assembly.GetManifestResourceStream("NRack.Mixups.JavaScript.common.js"))
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

        public static string ReadSource(string fileName)
        {
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            var sourceStorage = SourceStorage.Instance.Sources;
            if (!sourceStorage.ContainsKey(fileName))
            {
                using (var streamReader = new StreamReader(fileName))
                {
                    sourceStorage[fileName] = streamReader.ReadToEnd();
                }

                Context.SetGlobal("sourceStorage", SourceStorage.Instance);
            }

            var source = sourceStorage[fileName];

            return source;
        }
    }
}