using System;
using System.Collections.Generic;
using System.Text;
using Atlas.Core.Models;

namespace Atlas.Core.Executor;

public class PlanExecutor : IPlanExecutor
{
    private readonly ICommandExecutor _commandExecutor;

    public PlanExecutor(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    public async Task<PlanResult> ExecuteAsync(ExecutionPlan plan)
    {
        ArgumentNullException.ThrowIfNull(plan);

        var result = new PlanResult();

        foreach (var commandRequest in plan.Commands)
        {
            var commandResult =
                await _commandExecutor.ExecuteAsync(commandRequest);

            result.Results.Add(commandResult);

            if (!commandResult.Success)
            {
                result.Success = false;
                result.Message =
                    $"O plano falhou ao executar o comando '{commandRequest.commandName}'.";

                return result;
            }
        }

        result.Success = true;
        result.Message = "Plano executado com sucesso.";

        return result;
    }
}
