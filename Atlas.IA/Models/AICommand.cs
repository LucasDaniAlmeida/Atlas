using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Atlas.IA.Models;

public class AICommand
{
    public string CommandName { get; set; } = string.Empty;

    public Dictionary<string, JsonElement> Parameters { get; set; } = [];
}