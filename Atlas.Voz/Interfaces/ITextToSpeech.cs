using System;
using System.Collections.Generic;
using System.Text;
using Atlas.IA.Models;

namespace Atlas.Voz.Interfaces;

public interface ITextToSpeech
{
    Task SpeakAsync(
        string text,
        SpeechDirective? directive = null
    );
}