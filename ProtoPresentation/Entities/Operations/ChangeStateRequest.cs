namespace ProtoPresentation.Entities.Operations;

public record ChangeStateRequest(string State, string ParticipantId, string PresentationId = null);
