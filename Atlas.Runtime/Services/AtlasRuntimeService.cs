using System;
using System.Collections.Generic;
using System.Text;
using Atlas.IA.Models;
using Atlas.IA.Services;

namespace Atlas.Runtime.Services;

public class AtlasRuntimeService
{
    private readonly AtlasAIService _atlas;

    public AtlasRuntimeService(AtlasAIService atlas)
    {
        _atlas = atlas;
    }

    public async Task<AtlasInteractionResult> ProcessAsync(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        return await _atlas.Interaction.ProcessAsync(
            input,
            _atlas.Session
        );
    }
}