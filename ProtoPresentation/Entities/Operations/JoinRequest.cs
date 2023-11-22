namespace ProtoPresentation.Entities.Operations;

public record JoinRequest(string ParticipantId, string ParticipantName, bool IsPresenter, DateTime StartedAt, string PresentationId = null);
