using NRack.Configuration;

namespace Mixup.Web
{
    public class Config : ConfigBase
    {
        public override void Start()
        {
            Map("/",
                config => config.Run(new JavaScriptApp(@"app\hello_world.js")))
            .Map("/echo",
                config => config.Run(new JavaScriptApp(@"app\echo.js")));
        }
    }
}