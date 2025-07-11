namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> FindByIdAsync(Guid id);
        Task<IEnumerable<Question>> FindQuestionsByIdAsync(IEnumerable<Guid> ids);
        Task CreateAsync(Question question);
        Task CreateAsync(IEnumerable<Question> questions);
        void Remove(IEnumerable<Question> questions);
    }
}
