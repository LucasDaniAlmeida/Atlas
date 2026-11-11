using System;
using System.Collections.Generic;
using System.Text;
using Atlas.Core.Models;
using Atlas.IA.Conversation;
using Atlas.IA.Models;

namespace Atlas.IA.Interfaces;

public interface IAtlasAI
{
    Task<AtlasResponse> ProcessAsync(
        string input,
        ConversationSession session);

    Task<AtlasResponse> ProcessExecutionResultAsync(
    PlanResult result,
    ConversationSession session);


}