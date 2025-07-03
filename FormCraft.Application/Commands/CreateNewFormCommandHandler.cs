using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.RequestModels;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;

namespace FormCraft.Application.Commands
{
    public class CreateNewFormCommandHandler : ICommandHandler<CreateNewFormCommand>
    {
        private readonly IFormRepository _formRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITopicExistenceChecker _topicExisteceChecker;
        private readonly IUserRoleChecker _userRoleChecker;
        private readonly IUnitOfWork _unitOfWork;

        private readonly Guid _testAuthor = Guid.NewGuid();

        public CreateNewFormCommandHandler(
            IFormRepository formRepository,
            IQuestionRepository questionRepository,
            ITagRepository tagRepository,
            ITopicExistenceChecker topicExisteceChecker,
            IUserRoleChecker userRoleChecker,
            IUnitOfWork unitOfWork)
        {
            _formRepository = formRepository;
            _questionRepository = questionRepository;
            _tagRepository = tagRepository;
            _topicExisteceChecker = topicExisteceChecker;
            _userRoleChecker = userRoleChecker;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateNewFormCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            var tags = await GetExistenceAndCreateNewTags(request.Tags);
            var form = await CreateFormAsync(request, tags);
            await AddQuestionsAsync(form, request.Questions);
        }

        private void ValidateRequest(CreateNewFormCommand request)
        {
            if (!_topicExisteceChecker.IsExist(request.Topic))
                throw new ArgumentException("Topic name not exist");

            if (request.Questions.Count() <= 0)
                throw new ArgumentException("Question list cannot be null");
        }

        private async Task<List<Tag>> GetExistenceAndCreateNewTags(IEnumerable<string> tagNames)
        {
            List<Tag> tags = new List<Tag>();
            foreach (var tagName in tagNames)
            {
                if (string.IsNullOrWhiteSpace(tagName))
                    continue;

                var existenseTag = await _tagRepository.FindByNameAsync(tagName);
                if (existenseTag == null)
                {
                    existenseTag = new Tag(tagName);
                    await _tagRepository.AddAsync(existenseTag);
                    await _unitOfWork.CommitAsync();
                }
                if (existenseTag != null)
                {
                    tags.Add(existenseTag);
                }
            }
            return tags;
        }

        private async Task<Form> CreateFormAsync(CreateNewFormCommand request, List<Tag> tags)
        {
            var form = Form.Create(
                _testAuthor,
                request.Title,
                request.Description,
                request.ImageUrl,
                request.Topic,
                tags,
                request.IsPublic);

            await _formRepository.AddAsync(form);
            await _unitOfWork.CommitAsync();
            return await _formRepository.FindByIdAsync(form.Id)
                ?? throw new ArgumentException("Failed to retrieve newly created form.");
        }

        private async Task AddQuestionsAsync(Form form, IEnumerable<QuestionRequest> questions)
        {
            foreach (var question in questions)
            {
                var newQuestion = form.AddQuestion(
                    question.Text,
                    question.Type,
                    form.AuthorId,
                    _userRoleChecker);

                await _questionRepository.AddAsync(newQuestion);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
