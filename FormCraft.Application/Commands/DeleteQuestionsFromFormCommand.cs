using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands
{
    public record DeleteQuestionsFromFormCommand(Guid FormId, IEnumerable<Guid> QuestionIds) : ICommand;
}
