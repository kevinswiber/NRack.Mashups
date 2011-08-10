using System.Collections.Generic;
using IronJS;
using NRack.Helpers;

namespace NRack.Mixups.JavaScript
{
    public class JavaScriptAppResponseConverter
    {
        public dynamic[] ConvertJavaScriptResponse(CommonObject obj)
        {
            string status = null;
            var headerDictionary = new Hash();
            dynamic body = null;

            foreach (var prop in obj.Members)
            {
                if (prop.Key == "headers")
                {
                    var headers = prop.Value as CommonObject;

                    if (headers == null)
                    {
                        continue;
                    }

                    foreach (var header in headers.Members)
                    {
                        headerDictionary[header.Key] = header.Value.ToString();
                    }

                    continue;
                }

                if (prop.Key == "status")
                {
                    status = prop.Value.ToString();
                    continue;
                }

                if (prop.Key == "body")
                {
                    body = prop.Value;
                    if (body is ArrayObject)
                    {
                        var arr = (ArrayObject) prop.Value;
                        var responseBody = new List<string>();
                        for (var i  = 0; i < arr.Length; i++)
                        {
                            responseBody.Add(arr.Get(i).Object.ToString());
                        }
                        body = new IterableAdapter(responseBody);
                    }
                }
            }

            return new[] {status, headerDictionary, body};
        }
    }
}