using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Atlas.Core.Models;
using Atlas.IA.Models;

namespace Atlas.IA.Services;

public class PlanBuilder
{
    public ExecutionPlan Build(IEnumerable<AICommand> commands)
    {
        var plan = new ExecutionPlan();

        foreach (var aiCommand in commands)
        {
            var request = new CommandRequest
            {
                commandName = aiCommand.CommandName
            };

            foreach (var parameter in aiCommand.Parameters)
            {
                request.Parameters.Set(
                    parameter.Key,
                    ConvertJsonElement(parameter.Value)
                );
            }

            plan.Commands.Add(request);
        }

        return plan;
    }

    private static object ConvertJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String =>
                element.GetString() ?? string.Empty,

            JsonValueKind.Number when element.TryGetInt32(out var intValue) =>
                intValue,

            JsonValueKind.Number when element.TryGetInt64(out var longValue) =>
                longValue,

            JsonValueKind.Number =>
                element.GetDouble(),

            JsonValueKind.True =>
                true,

            JsonValueKind.False =>
                false,

            JsonValueKind.Null =>
                null!,

            _ =>
                element.GetRawText()
        };
    }
}