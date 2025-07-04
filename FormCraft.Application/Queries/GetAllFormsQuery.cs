using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.ViewModels;

namespace FormCraft.Application.Queries
{
    public record GetAllFormsQuery() : IQuery<IEnumerable<FormQuestionsView>>;
}
