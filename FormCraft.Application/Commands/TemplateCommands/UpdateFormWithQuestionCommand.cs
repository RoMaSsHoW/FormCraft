using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.RequestModels;

namespace FormCraft.Application.Commands.Template
{
    public record UpdateFormWithQuestionCommand(
        Guid FormId,
        string? Title,
        string? Description,
        string? TopicName,
        bool IsPublic,
        IEnumerable<string> Tags,
        IEnumerable<QuestionRequest> Questions) : ICommand;
}
