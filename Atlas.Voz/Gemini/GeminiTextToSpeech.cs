using Atlas.IA.Models;
using Atlas.Voz.Interfaces;
using Atlas.Voz.Models;
using System.Media;
using System.Text;
using System.Text.Json;

namespace Atlas.Voz.Gemini;

public class GeminiTextToSpeech : ITextToSpeech
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly VoiceProfile _profile;

    public GeminiTextToSpeech(VoiceProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        _profile = profile;

        _apiKey =
            Environment.GetEnvironmentVariable("GEMINI_API_KEY")
            ?? throw new InvalidOperationException(
                "A variável GEMINI_API_KEY não foi configurada."
            );

        _httpClient = new HttpClient();
    }

    public async Task SpeakAsync(
    string text,
    SpeechDirective? directive = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        var speechInstructions = $"""
            Idioma:
            {_profile.Language}

            Identidade da voz:
            {_profile.Identity}

            Ritmo padrão:
            {_profile.DefaultPace}

            Energia padrão:
            {_profile.DefaultEnergy}

            Direção específica desta fala:

            Emoção:
            {(
                _profile.AllowEmotion
                    ? directive?.Emotion ?? "use a emoção natural adequada ao texto"
                    : "não varie a emoção"
            )}

            Ritmo:
            {(
                _profile.AllowPaceVariation
                    ? directive?.Pace ?? _profile.DefaultPace
                    : _profile.DefaultPace
            )}

            Energia:
            {(
                _profile.AllowEnergyVariation
                    ? directive?.Energy ?? _profile.DefaultEnergy
                    : _profile.DefaultEnergy
            )}

            Estilo:
            {directive?.Style ?? "natural e conversacional"}

            Texto que deve ser falado:
            {text}
            """;


        var body = new
        {
            contents = new[]
    {
        new
        {
            parts = new[]
            {
                new
                {
                    text = speechInstructions
                }
            }
        }
    },

            generationConfig = new
            {
                responseModalities = new[] { "AUDIO" },

                speechConfig = new
                {
                    voiceConfig = new
                    {
                        prebuiltVoiceConfig = new
                        {
                            voiceName = _profile.VoiceName
                        }
                    }
                }
            }
        };

        string json = JsonSerializer.Serialize(body);

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-tts-preview:generateContent"
        );

        request.Headers.Add("x-goog-api-key", _apiKey);

        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        using var response =
            await _httpClient.SendAsync(request);

        string responseJson =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Erro no Gemini TTS: {responseJson}"
            );
        }

        using var document =
            JsonDocument.Parse(responseJson);

        string? base64Audio =
            document.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("inlineData")
                .GetProperty("data")
                .GetString();

        if (string.IsNullOrWhiteSpace(base64Audio))
            throw new InvalidOperationException(
                "O Gemini não retornou áudio."
            );

        byte[] pcm =
            Convert.FromBase64String(base64Audio);

        byte[] wav = CreateWav(pcm);

        using var stream = new MemoryStream(wav);

        using var player = new SoundPlayer(stream);

        player.PlaySync();
    }

    private static byte[] CreateWav(byte[] pcm)
    {
        const int sampleRate = 24000;
        const short channels = 1;
        const short bitsPerSample = 16;

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        int byteRate =
            sampleRate * channels * bitsPerSample / 8;

        short blockAlign =
            (short)(channels * bitsPerSample / 8);

        writer.Write(
            Encoding.ASCII.GetBytes("RIFF")
        );

        writer.Write(36 + pcm.Length);

        writer.Write(
            Encoding.ASCII.GetBytes("WAVE")
        );

        writer.Write(
            Encoding.ASCII.GetBytes("fmt ")
        );

        writer.Write(16);
        writer.Write((short)1);
        writer.Write(channels);
        writer.Write(sampleRate);
        writer.Write(byteRate);
        writer.Write(blockAlign);
        writer.Write(bitsPerSample);

        writer.Write(
            Encoding.ASCII.GetBytes("data")
        );

        writer.Write(pcm.Length);
        writer.Write(pcm);

        return stream.ToArray();
    }
}