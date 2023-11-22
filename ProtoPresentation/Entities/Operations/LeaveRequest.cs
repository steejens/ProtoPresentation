namespace ProtoPresentation.Entities.Operations;

public record LeaveRequest(string ParticipantId,DateTime StartedAt, string PresentationId = null);
