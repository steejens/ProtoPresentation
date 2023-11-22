
namespace ProtoPresentation.Entities.Operations;

public record GetStateResponse(StateResponse State);
public record StateResponse(Dictionary<string, string> Parts, string CurrentPresenterId, string State);