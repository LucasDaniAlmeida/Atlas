using Atlas.IA.Services;
using Atlas.Runtime.Services;
using Atlas.Voz.Gemini;
using Atlas.Voz.Models;
using Atlas.Voz.Services;

namespace Atlas.Testes;

[TestClass]
public class AtlasRuntimeTests
{
    [TestMethod]
    public async Task Deve_Falar_E_Executar()
    {
        var atlasAI = new AtlasAIService(
            "Data/conversation-history.json"
        );

        var runtime = new AtlasRuntimeService(atlasAI);

        await runtime.ProcessAsync(
            "Atlas, Abra o bloco que notas, escreva uma piada longa e conte ela"
        );
    }
}