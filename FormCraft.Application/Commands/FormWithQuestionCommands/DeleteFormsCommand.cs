using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands.FormWithQuestionCommands
{
    public record DeleteFormsCommand(IEnumerable<Guid> FormIds) : ICommand;
}
