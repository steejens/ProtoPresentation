using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Proto;
using Proto.Cluster;
using Proto.Persistence;
using ProtoPresentation.Entities.Operations;
using ProtoPresentation.Hubs;

namespace ProtoPresentation.Entities
{
    public class Presentation :IActor
    {
        private readonly IHubContext<PresentationHub> _eventsHubContext;

        public const string Kind = nameof(Presentation);

        public IContext Context { get; set; } = null!;
        private Cluster Cluster => Context.Cluster();
        private ClusterIdentity ClusterIdentity => Context.ClusterIdentity()!;
        private string Id => ClusterIdentity.Identity;
        private bool isCreated = false;
        private string PresentationName = null!;
        public Dictionary<string,string> Participants = new Dictionary<string,string>();
        public string State = string.Empty;
        public string CreatorId =string.Empty;
        public string CurrentPresenterId = string.Empty;

        public Presentation(IHubContext<PresentationHub> eventsHubContext)
        {
            _eventsHubContext = eventsHubContext;
        }

        public async Task ReceiveAsync(IContext context)
        {
            Context = context;

            switch (context.Message)
            {
                case CreateRequest request:
                    await CreateRequestHandler(request);
                    break;
                case JoinRequest request:
                    await Join(request);
                    break;
                case LeaveRequest request:
                    await Leave(request);
                    break;
                case ChangeStateRequest request:
                    await ChangeState(request);
                    break;
                case GetStateRequest request:
                    await GetState(request);
                    break;

            }
        }

        private async Task CreateRequestHandler(CreateRequest request)
        {
            if (!isCreated)
            {
                isCreated=  true;
                PresentationName = request.PresentationName;
                //CreatorId = request.CreatorId;
                //CurrentPresenterId = request.CreatorId;
                //AddParticipators(request.CreatorId,"Creator");
                Context.Respond(new CreateResponse(Id,PresentationName/*,CreatorId*/));
            }
        }

        private async Task Join(JoinRequest request)
        {
            if (isCreated)
            {
                if (request.IsPresenter)
                {
                    CurrentPresenterId = request.ParticipantId;
                    State = "1";
                }
                else
                {
                    await _eventsHubContext.Clients.Client(request.ParticipantId).SendAsync("SendMessage", State);
                }
                AddParticipators(request.ParticipantId, request.ParticipantName);
                Context.Respond(new OkResponse());

            }
            else
            {
                Context.Respond(new OkResponse("wrong req"));

            }

        }

        private async Task Leave(LeaveRequest request)
        {
            if (isCreated)
            {
                RemoveParticipators(request.ParticipantId);
                Context.Respond(new OkResponse());
            }
            else
            {
                Context.Respond(new OkResponse("wrong req"));

            }

        }


        private async Task ChangeState(ChangeStateRequest request)
        {
            if (isCreated)
            {
                if (CurrentPresenterId == request.ParticipantId)
                {
                    State = request.State;
                    Context.Respond(new OkResponse());

                    foreach (var participant in Participants.Keys)
                    {


                        // Send the state to the participant
                        await _eventsHubContext.Clients.Client(participant).SendAsync("SendMessage",request.State);
                    }

                }
                else
                {
                    Context.Respond(new OkResponse("sen kimsen"));
                }
            }
            else
            {
                Context.Respond(new OkResponse("wrong req"));

            }
        }

        private async Task GetState(GetStateRequest request)
        {
            if (isCreated)
            {
               
                Context.Respond(new GetStateResponse(new StateResponse(Participants,CurrentPresenterId,State)));

            }
        }

        private async Task GetPresentationState(GetPresentationStateRequest request)
        {
            if (isCreated)
            {

                await _eventsHubContext.Clients.Client(request.ParticipantId).SendAsync("SendMessage", State);

            }
        }

        private void AddParticipators(string key, string value)
        {
            Participants.TryAdd(key, value);
        }
        private void RemoveParticipators(string key)
        {
            if (Participants.ContainsKey(key))
            {
                Participants.Remove(key);
            }
        }
    }
}
