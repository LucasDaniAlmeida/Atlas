using System;
using System.Collections.Generic;
using System.Text;
using Atlas.Core.Models;

namespace Atlas.IA.Models;

public class AtlasResponse
{
    public string? Message { get; set; }

    public List<AICommand> Commands { get; set; } = [];

    public SpeechDirective? Speech { get; set; }
}