using System.Collections.Generic;
using Jurassic.Library;
using NRack.Helpers;

namespace Mixup
{
    public class JavaScriptAppResponseConverter
    {
        public dynamic[] ConvertJavaScriptResponse(ObjectInstance obj)
        {
            string status = null;
            var headerDictionary = new Hash();
            dynamic body = null;

            foreach (var prop in obj.Properties)
            {
                if (prop.Name == "headers")
                {
                    var headers = prop.Value as ObjectInstance;

                    if (headers == null)
                    {
                        continue;
                    }

                    foreach (var header in headers.Properties)
                    {
                        headerDictionary[header.Name] = header.Value.ToString();
                    }

                    continue;
                }

                if (prop.Name == "status")
                {
                    status = prop.Value.ToString();
                    continue;
                }

                if (prop.Name == "body")
                {
                    body = prop.Value;
                    if (body is ArrayInstance)
                    {
                        body = ((ArrayInstance) body).ElementValues;
                    }
                }
            }

            return new[] {status, headerDictionary, body};
        }
    }
}