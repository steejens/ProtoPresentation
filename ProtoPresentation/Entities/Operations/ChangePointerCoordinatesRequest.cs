namespace ProtoPresentation.Entities.Operations;

public record ChangePointerCoordinatesRequest(string PresentationId, string ParticipantId, double MouseX, double MouseY);


