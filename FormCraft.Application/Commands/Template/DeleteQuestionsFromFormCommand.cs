using FormCraft.Application.Common.Messaging;

namespace FormCraft.Application.Commands.Template
{
    public record DeleteQuestionsFromFormCommand(IEnumerable<Guid> QuestionIds) : ICommand;
}
