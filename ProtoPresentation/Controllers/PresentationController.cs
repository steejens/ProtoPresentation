using Microsoft.AspNetCore.Mvc;
using Proto;
using Proto.Cluster;
using ProtoPresentation.Entities;
using ProtoPresentation.Entities.Operations;

namespace ProtoPresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresentationController: ControllerBase
    {
        private readonly ActorSystem _actorSystem;

        public PresentationController(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([FromBody]CreateRequest request)
        {
           var presentationId = Guid.NewGuid().ToString();
            var response = await _actorSystem.Cluster()
                .RequestAsync<CreateResponse>(
                    kind: Presentation.Kind,
                    identity: presentationId,
                    message: request,
                    ct: CancellationToken.None
                );

            return Ok(response);
        }

        [HttpPost("Join/{id}")]
        public async Task<IActionResult> JoinRequest(string id,[FromBody] JoinRequest request)
        {
            var response = await _actorSystem.Cluster()
                .RequestAsync<OkResponse>(
                    kind: Presentation.Kind,
                    identity: id,
                    message: request,
                    ct: CancellationToken.None
                );

            return Ok(response);
        }


        [HttpPost("Leave/{id}")]
        public async Task<IActionResult> LeaveRequest(string id,[FromBody] LeaveRequest request)
        {
            var response = await _actorSystem.Cluster()
                .RequestAsync<OkResponse>(
                    kind: Presentation.Kind,
                    identity: id,
                    message: request,
                    ct: CancellationToken.None
                );

            return Ok(response);
        }

        [HttpPost("change-state/{id}")]
        public async Task<IActionResult> ChangeStateRequest(string id, [FromBody] ChangeStateRequest request)
        {
            var response = await _actorSystem.Cluster()
                .RequestAsync<OkResponse>(
                    kind: Presentation.Kind,
                    identity: id,
                    message: request,
                    ct: CancellationToken.None
                );

            return Ok(response);
        }

        [HttpGet("State/{id}")]
        public async Task<IActionResult> GetState(string id,[FromQuery] GetStateRequest request)
        {
            var response = await _actorSystem.Cluster()
                .RequestAsync<GetStateResponse>(
                    kind: Presentation.Kind,
                    identity: id,
                    message: request,
                    ct: CancellationToken.None
                );

            return Ok(response);
        }
    }
}
