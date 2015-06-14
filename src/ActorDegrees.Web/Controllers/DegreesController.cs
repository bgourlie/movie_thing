using System.Web.Http;

namespace ActorDegrees.Web.Controllers
{
    using System.Net;
    using System.Net.Http;

    [RoutePrefix("api/degrees")]
    public class DegreesController : ApiController
    {

        public HttpResponseMessage Get(int actor1, int actor2)
        {
            var path = WebApiApplication.Graph.GetShortestPath(actor1, actor2);
            return Request.CreateResponse(HttpStatusCode.OK, new { Result = "Hello"});
        }
    }
}
