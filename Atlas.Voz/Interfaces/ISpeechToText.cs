using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Voz.Interfaces;

public interface ISpeechToText
{
    Task<string> TranscribeAsync(string audioFilePath);
}