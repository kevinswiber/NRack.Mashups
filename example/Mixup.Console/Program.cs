using System;
using Jurassic.Library;


namespace Mixup.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var evaluator = new Mixup.JavaScriptEvaluator();
            var obj = evaluator.Evaluate();

            foreach(var prop in obj.Properties)
            {
                if (prop.Name == "headers")
                {
                    var headers = prop.Value as ObjectInstance;

                    if (headers == null)
                    {
                        continue;
                    }

                    System.Console.WriteLine("headers");
                    foreach(var header in headers.Properties)
                    {
                        System.Console.WriteLine("\t" + header.Name + " => " + header.Value);
                    }

                    continue;
                }

                System.Console.WriteLine(prop.Name + " => " + prop.Value);
            }

            System.Console.ReadLine();
        }
    }
}
