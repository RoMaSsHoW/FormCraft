using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.RequestModels;

namespace FormCraft.Application.Commands.Template
{
    public record CreateNewFormWithQuestionCommand(
        string Title,
        string Description,
        string Topic,
        bool IsPublic,
        IEnumerable<string> Tags,
        IEnumerable<QuestionRequest> Questions) : ICommand;
}
