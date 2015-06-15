using System.Web;
using System.Web.Http;

namespace ActorDegrees.Web
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using DataTypes;

    public class WebApiApplication : HttpApplication
    {
        public static ReadonlyGraph Graph { get; private set; }
        public static Dictionary<int, string>  Movies { get; private set; }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            using (var stream = File.OpenRead(Server.MapPath("out.bin")))
            {
                Graph = ReadonlyGraph.NewFromStream(stream);
            }

            using (var stream = File.OpenRead(Server.MapPath("movies.bin")))
            {
                Movies = LoadMoviesTable(stream);
            }
        }

        private static Dictionary<int, string> LoadMoviesTable(Stream stream)
        {
            var moviesTable = new Dictionary<int, string>();
            using (var reader = new BinaryReader(stream, Encoding.Unicode))
            {
                var numEntries = reader.ReadInt32();

                for (int i = 0; i < numEntries; ++i)
                {
                    var movieId = reader.ReadInt32();
                    var titleLength = reader.ReadInt32();
                    var titleBytes = reader.ReadChars(titleLength);
                    moviesTable.Add(movieId, new string(titleBytes));
                }
            }
            return moviesTable;
        }
    }
}