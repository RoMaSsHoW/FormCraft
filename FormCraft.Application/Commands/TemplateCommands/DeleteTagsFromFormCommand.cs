using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands.Template
{
    public record DeleteTagsFromFormCommand(Guid FormId, IEnumerable<Guid> TagIds) : ICommand;
}
