using System.Web.Http;

namespace ActorDegrees.Web.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    [RoutePrefix("api/degrees")]
    public class DegreesController : ApiController
    {

        public HttpResponseMessage Get(int actor1, int actor2)
        {
            var path = WebApiApplication.Graph.GetShortestPath(actor1, actor2);
            var transformed = path.Select(
                p =>
                    new PathEntry(WebApiApplication.Graph.GetActorById(p.Node), p.Edge.MovieId, WebApiApplication.Movies[p.Edge.MovieId]));

            return Request.CreateResponse(HttpStatusCode.OK, transformed);
        }
    }

    public class PathEntry
    {
        public readonly int MovieId;
        public readonly string ActorName;
        public readonly string MovieName;

        public PathEntry(string actorName, int movieId, string movieName)
        {
            MovieId = movieId;
            ActorName = actorName;
            MovieName = movieName;
        }
    }
}
