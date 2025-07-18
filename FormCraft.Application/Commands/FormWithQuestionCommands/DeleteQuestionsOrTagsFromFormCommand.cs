using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands.FormWithQuestionCommands
{
    public record DeleteQuestionsOrTagsFromFormCommand(
        Guid FormId,
        IEnumerable<Guid> QuestionIds,
        IEnumerable<Guid> TagIds) : ICommand;
}
