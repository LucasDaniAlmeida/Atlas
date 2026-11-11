using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Voz.Models;

public class VoiceProfile
{
    public string VoiceName { get; set; } = "Kore";

    public string Language { get; set; } = "pt-BR";

    public string Identity { get; set; } =
        "Voz amigável, descontraída e conversacional.";

    public string DefaultPace { get; set; } = "normal";

    public string DefaultEnergy { get; set; } = "moderada";

    public bool AllowEmotion { get; set; } = true;

    public bool AllowPaceVariation { get; set; } = true;

    public bool AllowEnergyVariation { get; set; } = true;

    public bool AllowHumor { get; set; } = true;
}