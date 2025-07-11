using FormCraft.Application.Models.ViewModels;

namespace FormCraft.Application.Models.RequestModels;

public class UpdateTemplateRequest
{
    public TemplateView NewTemplateInformation { get; init; }
    public byte[] RowVersion { get; init; }
}