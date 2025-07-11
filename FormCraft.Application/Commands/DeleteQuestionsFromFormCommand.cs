using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands
{
    public record DeleteQuestionsFromFormCommand(IEnumerable<Guid> QuestionIds) : ICommand;
}
