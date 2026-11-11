using System;
using System.Collections.Generic;
using System.Text;
using Atlas.Core.Commands.Interfaces;

namespace Atlas.Core.Registry
{
    public interface ICommandRegistry
    {
        void Register(ICommand command);
        bool TryGet(
            string commandName, out ICommand? command);
    }
}
