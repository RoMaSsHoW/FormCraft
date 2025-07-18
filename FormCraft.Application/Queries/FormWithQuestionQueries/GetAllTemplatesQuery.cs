using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.ViewModels;

namespace FormCraft.Application.Queries.FormWithQuestionQueries
{
    public record GetAllTemplatesQuery(
        string? TagName = null,
        string? TopicName = null,
        string? TitleSearch = null) : IQuery<IEnumerable<TemplateView>>;
}
