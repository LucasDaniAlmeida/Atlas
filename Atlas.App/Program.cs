using Atlas.IA.Services;
using Atlas.Runtime.Services;

Console.Title = "Atlas OS";

Console.WriteLine("Atlas OS");
Console.WriteLine("Digite 'sair' para encerrar.");
Console.WriteLine();

var dataDirectory = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "AtlasOS"
);

Directory.CreateDirectory(dataDirectory);

var historyFile = Path.Combine(
    dataDirectory,
    "conversation-history.json"
);

var atlasAI = new AtlasAIService(historyFile);
var runtime = new AtlasRuntimeService(atlasAI);

while (true)
{
    Console.Write("Você > ");

    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (input.Equals("sair", StringComparison.OrdinalIgnoreCase))
        break;

    try
    {
        var result = await runtime.ProcessAsync(input);

        if (!string.IsNullOrWhiteSpace(result.Response.Message))
        {
            Console.WriteLine();
            Console.WriteLine($"Atlas > {result.Response.Message}");
        }

        if (result.ExecutionResult is not null)
        {
            if (!result.ExecutionResult.Success)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"[Falha] {result.ExecutionResult.Message}"
                );
            }
        }

        if (!string.IsNullOrWhiteSpace(result.Feedback?.Message))
        {
            Console.WriteLine();
            Console.WriteLine($"Atlas > {result.Feedback.Message}");
        }

        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine($"[Erro] {ex.Message}");
        Console.WriteLine();
    }
}