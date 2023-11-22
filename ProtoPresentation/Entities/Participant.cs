using Microsoft.AspNetCore.SignalR;
using Proto;
using Proto.Cluster;
using Proto.Persistence;
using ProtoPresentation.Entities.Operations;
using ProtoPresentation.Hubs;
using static Proto.Cluster.IdentityHandoverAck.Types;

namespace ProtoPresentation.Entities
{
    public class Participant :IActor
    {
        public const string Kind = nameof(Participant);

        public IContext Context { get; set; } = null!;
        private Cluster Cluster => Context.Cluster();
        private ClusterIdentity ClusterIdentity => Context.ClusterIdentity()!;
        private string Id => ClusterIdentity.Identity;
        private string PersistenceId => $"{Kind}/{Id}";
        private Persistence _persistence = null!;
 
        private string ParticipantName { get; set; } = null!;
        private string ParticipantId { get; set; } = null!;
        private bool IsPresenter = false;
        private IHubContext<PresentationHub> HubContext;
        private enum ParticipantStatus { Joined, Disconnected }
        private ParticipantStatus _participantState = ParticipantStatus.Disconnected;

        public Participant(string connectionId, IHubContext<PresentationHub> hubContext)
        {
            ParticipantId = connectionId;
            HubContext = hubContext;
        }

        public async Task ReceiveAsync(IContext context)
        {
            Context = context;

            switch (context.Message)
            {
                case Started st:
                    break;

                case JoinRequest joinRequest:
                    if (!joinRequest.IsPresenter)
                    {
                        await ParticipantJoinPresentationHandler(joinRequest);
                    }
                    else
                    {
                        IsPresenter = joinRequest.IsPresenter;
                        await PresenterJoinPresentationHandler(joinRequest);
                    }

                    break;
                case LeaveRequest leaveRequest:
                    await ParticipantLeavePresentationHandler(leaveRequest);
                    break;
                case ChangeStateRequest changeRequest:
                    await ChangeStateHandler(changeRequest);
                    break;
                case ChangePointerCoordinatesRequest request:
                    await ChangePointerCoordinatesHandler(request);
                    break;
            }
        }

        public async Task ParticipantJoinPresentationHandler(JoinRequest request)
        {
            var response = await Cluster
                .RequestAsync<OkResponse>(
                    kind: Presentation.Kind,
                    identity: request.PresentationId,
                    message: request,
                    ct: CancellationToken.None
                );
        }


        public async Task PresenterJoinPresentationHandler(JoinRequest request)
        {
            var response = await Cluster
                .RequestAsync<OkResponse>(
                    kind: Presentation.Kind,
                    identity: request.PresentationId,
                    message: request,
                    ct: CancellationToken.None
                );
        }
        public async Task ParticipantLeavePresentationHandler(LeaveRequest request)
        {
            var response = await Cluster
                .RequestAsync<OkResponse>(
                    kind: Presentation.Kind,
                    identity: request.PresentationId,
                    message: request,
                    ct: CancellationToken.None
                );
        }


        public async Task ChangeStateHandler(ChangeStateRequest request)
        {
            var response = await Cluster
                .RequestAsync<OkResponse>(
                    kind: Presentation.Kind,
                    identity: request.PresentationId,
                    message: request,
                    ct: CancellationToken.None
                );
        }

        public async Task ChangePointerCoordinatesHandler(ChangePointerCoordinatesRequest request)
        {
            var response = await Cluster
                .RequestAsync<OkResponse>(
                    kind: Presentation.Kind,
                    identity: request.PresentationId,
                    message: request,
                    ct: CancellationToken.None
                );
        }
    }
}
