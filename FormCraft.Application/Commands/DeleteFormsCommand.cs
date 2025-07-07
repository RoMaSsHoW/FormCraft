using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands
{
    public record DeleteFormsCommand(IEnumerable<Guid> FormIds) : ICommand;
}
