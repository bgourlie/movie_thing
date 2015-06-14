using System.Web;
using System.Web.Http;

namespace ActorDegrees.Web
{
    using System.IO;
    using DataTypes;

    public class WebApiApplication : HttpApplication
    {
        public static ReadonlyGraph Graph { get; private set; }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            using (var stream = File.OpenRead(Server.MapPath("out.bin")))
            {
                Graph = ReadonlyGraph.NewFromStream(stream);
            }
        }
    }
}