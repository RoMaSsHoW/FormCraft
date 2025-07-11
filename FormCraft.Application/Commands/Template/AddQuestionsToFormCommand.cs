using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.DTO;

namespace FormCraft.Application.Commands.Template
{
    public record AddQuestionsToFormCommand(
        Guid FormId,
        IEnumerable<QuestionDTO> Questions) : ICommand;
}
