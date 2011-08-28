using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IronJS;
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

        private static void IncludeAssemblies(IEnumerable<Assembly> assemblies)
        {
            foreach(var assembly in assemblies)
            {
                IncludeAssembly(assembly);
            }
        }

        private static void IncludeAssembly(Assembly assembly)
        {
            foreach(var type in assembly.GetExportedTypes())
            {
                var names = type.FullName.Split('.');

                var table = Context.Globals;
                for (var i = 0; i < names.Length - 1; i++)
                {
                    var name = names[i];
                    if (table.Members.ContainsKey(name))
                    {
                        table = (CommonObject)table.Members[name];
                    }
                    else
                    {
                        var tmp = table.Env.NewObject();
                        table.Put(name, tmp);
                        table = tmp;
                    }
                }

                table.Put(names[names.Length - 1], type);
            }
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
            //var sourceStorage = SourceStorage.Instance.Sources;
            //if (!sourceStorage.ContainsKey(fileName))
            //{
            //    sourceStorage[fileName] = ReadFile(fileName);
            //    Context.SetGlobal("sourceStorage", SourceStorage.Instance);
            //}

            //var source = sourceStorage[fileName];
            var source = ReadFile(fileName);

            return source;
        }

        private static string ReadFile(string fileName)
        {
            string contents;

            using (var streamReader = new StreamReader(fileName))
            {
                contents = streamReader.ReadToEnd();
            }

            return contents;
        }
    }
}