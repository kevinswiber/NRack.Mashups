using System;
using System.Collections.Generic;
using System.IO;
using NRack;

namespace Mixup
{
    public class JavaScriptApp : ICallable
    {
        private static readonly IDictionary<string, string> SourceStorage = new Dictionary<string, string>();
        private readonly string _javaScriptSource;

        public JavaScriptApp(string fileName)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            fileName = Path.Combine(baseDir, fileName);

            if (!SourceStorage.ContainsKey(fileName))
            {
                using (var streamReader = new StreamReader(fileName))
                {
                    SourceStorage[fileName] = streamReader.ReadToEnd();
                }
            }

            _javaScriptSource = SourceStorage[fileName];            
        }

        public dynamic[] Call(IDictionary<string, dynamic> environment)
        {
            var js = new JavaScriptEvaluator().Evaluate(_javaScriptSource, environment);
            return new JavaScriptAppResponseConverter().ConvertJavaScriptResponse(js);
        }
    }
}