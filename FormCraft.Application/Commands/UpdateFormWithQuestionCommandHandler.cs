using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands;

public class UpdateFormWithQuestionCommandHandler : ICommandHandler<UpdateFormWithQuestionCommand, byte[]>
{
    private readonly IFormRepository _formRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ITopicExistenceChecker _topicExistenceChecker;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFormWithQuestionCommandHandler(
        IFormRepository formRepository,
        IQuestionRepository questionRepository,
        ITagRepository tagRepository,
        ITopicExistenceChecker topicExistenceChecker,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _formRepository = formRepository;
        _questionRepository = questionRepository;
        _tagRepository = tagRepository;
        _topicExistenceChecker = topicExistenceChecker;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<byte[]> Handle(UpdateFormWithQuestionCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        var form = await _formRepository.FindByIdAsync(request.NewTemplateInformation.Id);
        if (form is null)
            throw new ArgumentException("Form not found.");

        _formRepository.SetOriginalRowVersion(form, request.RowVersion);

        await UpdateFormAsync(form, request);
        await UpdateQuestionsAsync(request);

        await _unitOfWork.CommitAsync(cancellationToken);
        
        return form.RowVersion;
    }

    private void ValidateRequest(UpdateFormWithQuestionCommand request)
    {
        if (!_currentUserService.IsAuthenticated())
            throw new UnauthorizedAccessException("User unauthorized.");

        if (!_topicExistenceChecker.IsExist(request.NewTemplateInformation.TopicName))
            throw new ArgumentException("Topic name does not exist.");

        if (!request.NewTemplateInformation.Questions.Any())
            throw new ArgumentException("Question list cannot be empty.");
    }

    private async Task UpdateFormAsync(Domain.Aggregates.FormAggregate.Form form, UpdateFormWithQuestionCommand request)
    {
        var info = request.NewTemplateInformation;

        form.ChangeTitle(info.Title, _currentUserService);
        form.ChangeDescription(info.Description, _currentUserService);
        form.ChangeTopic(info.TopicName, _topicExistenceChecker, _currentUserService);
        form.ChangeVisibility(info.IsPublic, _currentUserService);

        var tags = await GetOrCreateTagsAsync(info.Tags);

        foreach (var tag in tags)
        {
            form.AddTag(tag, _currentUserService);
        }
    }

    private async Task<IEnumerable<Tag>> GetOrCreateTagsAsync(IEnumerable<string> tagNames)
    {
        var tags = new List<Tag>();

        foreach (var tagName in tagNames.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var existingTag = await _tagRepository.FindByNameAsync(tagName);
            if (existingTag != null)
            {
                tags.Add(existingTag);
            }
            else
            {
                var newTag = new Tag(tagName);
                await _tagRepository.CreateAsync(newTag);
                tags.Add(newTag);
            }
        }

        return tags;
    }

    private async Task UpdateQuestionsAsync(UpdateFormWithQuestionCommand request)
    {
        var questions = request.NewTemplateInformation.Questions;

        for (int i = 0; i < questions.Count; i++)
        {
            var questionDto = questions[i];
            if (questionDto.Id == null)
                throw new ArgumentException("Question ID is required for update.");

            var existingQuestion = await _questionRepository.FindByIdAsync((Guid)questionDto.Id);
            if (existingQuestion == null)
                throw new ArgumentException($"Question with ID {questionDto.Id} not found.");

            existingQuestion.ChangeText(questionDto.Text, _currentUserService);
            existingQuestion.ChangeType(questionDto.Type, _currentUserService);
            existingQuestion.ChangeOrderNumber(i + 1, _currentUserService);
        }
    }
}