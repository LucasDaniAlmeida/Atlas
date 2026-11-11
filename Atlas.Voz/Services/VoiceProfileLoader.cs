using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Atlas.Voz.Models;

namespace Atlas.Voz.Services;

public class VoiceProfileLoader
{
    public async Task<VoiceProfile> LoadAsync(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "O arquivo de perfil de voz não foi encontrado.",
                filePath
            );
        }

        var json = await File.ReadAllTextAsync(filePath);

        var profile =
            JsonSerializer.Deserialize<VoiceProfile>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

        return profile
            ?? throw new InvalidOperationException(
                "Não foi possível carregar o perfil de voz."
            );
    }
}