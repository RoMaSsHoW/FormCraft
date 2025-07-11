using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.DTO;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands
{
    public class CreateNewFormWithQuestionCommandHandler : ICommandHandler<CreateNewFormWithQuestionCommand>
    {
        private readonly IFormRepository _formRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITopicExistenceChecker _topicExisteceChecker;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateNewFormWithQuestionCommandHandler(
            IFormRepository formRepository,
            IQuestionRepository questionRepository,
            ITagRepository tagRepository,
            ITopicExistenceChecker topicExisteceChecker,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _formRepository = formRepository;
            _questionRepository = questionRepository;
            _tagRepository = tagRepository;
            _topicExisteceChecker = topicExisteceChecker;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateNewFormWithQuestionCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var tags = await GetOrCreateTagsAsync(request.Tags);

            var form = await CreateFormAsync(request, tags);

            await AddQuestionsAsync(form, request.Questions);

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(CreateNewFormWithQuestionCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            if (!_topicExisteceChecker.IsExist(request.Topic))
                throw new ArgumentException("Topic name not exist");

            if (!request.Questions.Any())
                throw new ArgumentException("Question list cannot be null");
        }

        private async Task<IEnumerable<Tag>> GetOrCreateTagsAsync(IEnumerable<string> tagNames)
        {
            var tags = new List<Tag>();

            foreach (var tagName in tagNames)
            {
                var existenceTag = await _tagRepository.FindByNameAsync(tagName);
                if (existenceTag != null)
                {
                    tags.Add(existenceTag);
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
            var userId = _currentUserService.GetUserId();

            var form = Form.Create(
                (Guid)userId!,
                request.Title,
                request.Description,
                request.Topic,
                tags,
                request.IsPublic);

            await _formRepository.AddAsync(form);
            return form;
        }

        private async Task AddQuestionsAsync(Form form, IEnumerable<QuestionDTO> questions)
        {
            var newQuestions = new List<Question>();
            var existingQuestions = form.Questions;
            var lastOrderNumber = existingQuestions.Any() ? existingQuestions.Max(q => q.OrderNumber) : 0;

            foreach (var question in questions)
            {
                var type = QuestionType.FromName<QuestionType>(question.Type);

                if (existingQuestions.Any(q => q.Text == question.Text && q.Type == type))
                    continue;

                var newQuestion = Question.Create(form.Id, form.AuthorId, question.Text, question.Type, lastOrderNumber + 1);

                newQuestions.Add(newQuestion);
            }

            if (newQuestions.Any())
            {
                await _questionRepository.CreateAsync(newQuestions);
            }
        }
    }
}
