using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.DTO;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands;

public class CreateNewFormWithQuestionCommandHandler : ICommandHandler<CreateNewFormWithQuestionCommand>
{
    private readonly IFormRepository _formRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ITopicExistenceChecker _topicExistenceChecker;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateNewFormWithQuestionCommandHandler(
        IFormRepository formRepository,
        ITagRepository tagRepository,
        ITopicExistenceChecker topicExistenceChecker,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _formRepository = formRepository;
        _tagRepository = tagRepository;
        _topicExistenceChecker = topicExistenceChecker;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateNewFormWithQuestionCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        var tags = await GetOrCreateTagsAsync(request.Tags);
        var form = await CreateFormAsync(request, tags);

        AddQuestions(form, request.Questions);

        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private void ValidateRequest(CreateNewFormWithQuestionCommand request)
    {
        if (!_currentUserService.IsAuthenticated())
            throw new UnauthorizedAccessException("User is not authenticated.");

        if (!_topicExistenceChecker.IsExist(request.Topic))
            throw new ArgumentException($"Topic '{request.Topic}' does not exist.", nameof(request.Topic));

        if (request.Questions == null || !request.Questions.Any())
            throw new ArgumentNullException(nameof(request.Questions), "Question list cannot be null or empty.");
    }

    private async Task<IEnumerable<Tag>> GetOrCreateTagsAsync(IEnumerable<string> tagNames)
    {
        var tags = new List<Tag>();

        foreach (var tagName in tagNames.Distinct())
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

    private async Task<Form> CreateFormAsync(CreateNewFormWithQuestionCommand request, IEnumerable<Tag> tags)
    {
        var userId = _currentUserService.GetUserId()
                     ?? throw new InvalidOperationException("User ID could not be resolved.");

        var form = Form.Create(
            userId,
            request.Title,
            request.Description,
            request.Topic,
            tags,
            request.IsPublic);

        await _formRepository.AddAsync(form);
        return form;
    }

    private void AddQuestions(Form form, IEnumerable<QuestionDTO> questions)
    {
        int order = form.Questions.Any() 
            ? form.Questions.Max(q => q.OrderNumber) 
            : 0;

        foreach (var question in questions)
        {
            form.AddQuestion(question.Text, question.Type, ++order);
        }
    }
}
