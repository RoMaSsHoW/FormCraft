using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.RequestModels;

namespace FormCraft.Application.Commands.Template
{
    public record AddQuestionsToFormCommand(
        Guid FormId,
        IEnumerable<QuestionRequest> Questions) : ICommand;
}
