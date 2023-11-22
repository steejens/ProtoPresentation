using Microsoft.AspNetCore.SignalR;
using Proto;
using ProtoPresentation.Entities;
using ProtoPresentation.Entities.Operations;

namespace ProtoPresentation.Hubs
{
    public class PresentationHub: Hub
    {
        private readonly IHubContext<PresentationHub> _eventsHubContext;
        private readonly ActorSystem _actorSystem;

        public PresentationHub(IHubContext<PresentationHub> eventsHubContext, ActorSystem actorSystem)
        {
            _eventsHubContext = eventsHubContext;
            _actorSystem = actorSystem;
        }
        private PID? ConnectionPID
        {
            get => Context.Items["connection-pid"] as PID;
            set => Context.Items["connection-pid"] = value;
        }
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            var a = Props.FromProducer(() => new Participant(connectionId, _eventsHubContext));
            ConnectionPID = _actorSystem.Root.Spawn(a);
        }
        public void Join(string prId, string name)
        {
            if (ConnectionPID != null)
            {
                _actorSystem.Root.Send(ConnectionPID,
                    new JoinRequest(ParticipantId: Context.ConnectionId, ParticipantName: name, IsPresenter:false, StartedAt: DateTime.Now, prId));
            }
        }

        public void JoinAsPresenter(string prId, string name)
        {
            if (ConnectionPID != null)
            {
                _actorSystem.Root.Send(ConnectionPID,
                    new JoinRequest(ParticipantId: Context.ConnectionId, ParticipantName: name, IsPresenter: true, StartedAt: DateTime.Now, prId));
            }
        }

        public void Leave(string prId)
        {
            if (ConnectionPID != null)
            {
                _actorSystem.Root.Send(ConnectionPID,
                    new LeaveRequest(ParticipantId: Context.ConnectionId, StartedAt: DateTime.Now, prId));
            }
        }


        public void ChangeState(string state, string participantId, string prId)
        {
            if (ConnectionPID != null)
            {
                _actorSystem.Root.Send(ConnectionPID,
                    new ChangeStateRequest(State:state,ParticipantId: Context.ConnectionId,prId));
            }
        }

        public void ChangePointerCoordinates(string prId, double mouseX, double mouseY)
        {
            if (ConnectionPID != null)
            {
                _actorSystem.Root.Send(ConnectionPID,
                    new ChangePointerCoordinatesRequest(PresentationId:prId, ParticipantId: Context.ConnectionId, mouseX, mouseY));
            }
        }

    }
}
