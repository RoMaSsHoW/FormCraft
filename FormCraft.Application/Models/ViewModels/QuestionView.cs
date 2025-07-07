namespace FormCraft.Application.Models.ViewModels
{
    public class QuestionView
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int? OrderNumber { get; set; }
    }
}
