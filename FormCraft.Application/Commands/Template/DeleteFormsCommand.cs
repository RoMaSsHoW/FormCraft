using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands.Template
{
    public record DeleteFormsCommand(IEnumerable<Guid> FormIds) : ICommand;
}
