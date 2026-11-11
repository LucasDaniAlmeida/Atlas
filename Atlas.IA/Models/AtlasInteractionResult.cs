using Atlas.Core.Models;

namespace Atlas.IA.Models;

public class AtlasInteractionResult
{
    public AtlasResponse Response { get; set; } = new();

    public PlanResult? ExecutionResult { get; set; }

    public AtlasResponse? Feedback { get; internal set; }
}
