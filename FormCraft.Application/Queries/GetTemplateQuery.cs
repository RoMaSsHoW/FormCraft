using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.ViewModels;

namespace FormCraft.Application.Queries
{
    public record GetTemplateQuery(Guid TemplateId) : IQuery<TemplateView>;
}
