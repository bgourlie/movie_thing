using Microsoft.AspNet.Mvc;

namespace ActorDegrees.Api
{
    [Route("api/degrees")]
    public class ApiController
    {
        [HttpGet]
        public IActionResult GetDegrees()
        {
            return new ObjectResult(new {Thing="hello"});
        }
    }
}
