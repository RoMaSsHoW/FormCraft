namespace FormCraft.Application.Models.ViewModels
{
    public class QuestionView
    {
        public Guid? Id { get; init; } = null;
        public string Text { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public int OrderNumber { get; init; } = 0;
    }
}
