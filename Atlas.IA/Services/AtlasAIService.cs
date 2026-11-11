using Atlas.Core.Commands.Keyboard;
using Atlas.Core.Commands.Mouse;
using Atlas.Core.Commands.System;
using Atlas.Core.Executor;
using Atlas.Core.Registry;
using Atlas.IA.Conversation;
using Atlas.IA.Gemini;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.IA.Services;

public class AtlasAIService
{
    public ConversationSession Session { get; }

    public AtlasInteractionService Interaction { get; }

    public AtlasAIService(string historyFilePath)
    {
        var history =
            new JsonConversationHistory(historyFilePath);

        Session =
            new ConversationSession();

        var ai =
            new GeminiAtlasAI(history);

        var registry =
            new CommandRegistry();

        registry.Register(new OpenApplication());
        registry.Register(new Wait());
        registry.Register(new WriteText());
        registry.Register(new Click());
        registry.Register(new MoveMouse());
        registry.Register(new PressKey());
        registry.Register(new Hotkey());

        var commandExecutor =
            new CommandExecutor(registry);

        var planExecutor =
            new PlanExecutor(commandExecutor);

        var planBuilder =
            new PlanBuilder();

        Interaction =
            new AtlasInteractionService(
                ai,
                planBuilder,
                planExecutor
            );
    }
}