using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands.TemplateCommands
{
    public record DeleteQuestionsOrTagsFromFormCommand(
        Guid FormId,
        IEnumerable<Guid> QuestionIds,
        IEnumerable<Guid> TagIds) : ICommand;
}
