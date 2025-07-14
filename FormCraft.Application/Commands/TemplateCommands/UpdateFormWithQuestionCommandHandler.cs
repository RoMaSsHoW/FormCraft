using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands.Template
{
    public class UpdateFormWithQuestionCommandHandler : ICommandHandler<UpdateFormWithQuestionCommand>
    {
        private readonly IFormRepository _formRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITopicExistenceChecker _topicExisteceChecker;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFormWithQuestionCommandHandler(
            IFormRepository formRepository,
            ITagRepository tagRepository,
            ITopicExistenceChecker topicExisteceChecker,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _formRepository = formRepository;
            _tagRepository = tagRepository;
            _topicExisteceChecker = topicExisteceChecker;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateFormWithQuestionCommand request, CancellationToken cancellationToken)
        {
            var form = await ValidateRequest(request);

            if (form.Xmin != request.LastVersion)
                throw new ArgumentException("Form has been modified by another user. Please refresh and try again.");

            form = await ChangeFormAsync(form, request);

            CreateNewQuestions(form, request);

            await _unitOfWork.CommitAsync();

            ChangeQuestions(form, request);

            form.SetLastModifiedNow(_currentUserService);

            await _unitOfWork.CommitAsync();

            //ChangeQuestions(form, request);

            //await _unitOfWork.CommitAsync();
        }

        private async Task<Form> ValidateRequest(UpdateFormWithQuestionCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            if (!string.IsNullOrWhiteSpace(request.TopicName))
                if (!_topicExisteceChecker.IsExist(request.TopicName))
                    throw new ArgumentException("Topic name not exist");

            var form = await _formRepository.FindByIdAsync(request.FormId);
            if (form == null)
                throw new ArgumentException("Form not found");

            if (!request.Questions.Any())
                throw new ArgumentException("Question list cannot be null");

            return form;
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

        private async Task<Form> ChangeFormAsync(Form form, UpdateFormWithQuestionCommand request)
        {
            if (!string.IsNullOrWhiteSpace(request.Title))
                form.ChangeTitle(request.Title, _currentUserService);

            if (!string.IsNullOrWhiteSpace(request.Description))
                form.ChangeDescription(request.Description, _currentUserService);

            if (!string.IsNullOrWhiteSpace(request.TopicName))
                form.ChangeTopic(request.TopicName, _topicExisteceChecker, _currentUserService);

            form.ChangeVisibility(request.IsPublic, _currentUserService);

            var tags = await GetOrCreateTagsAsync(request.Tags);

            if (tags.Any())
                foreach (var tag in tags)
                    form.AddTag(tag, _currentUserService);

            return form;
        }

        private void CreateNewQuestions(Form form, UpdateFormWithQuestionCommand request)
        {
            foreach (var question in request.Questions.Where(q => q.Id == null))
            {
                form.AddQuestion(question.Text, question.Type, _currentUserService);
                var lastQuestion = form.Questions.Last(q => q.Text == question.Text && q.Type == QuestionType.FromName<QuestionType>(question.Type));
                question.Id = lastQuestion.Id;
            }
        }

        private void ChangeQuestions(Form form, UpdateFormWithQuestionCommand request)
        {
            var questionList = request.Questions.ToList();
            for (var i = 0; i < questionList.Count; i++)
            {
                var question = questionList[i];
                var existenceQuestion = form.Questions.First(q => q.Id == question.Id);
                if (existenceQuestion == null)
                    throw new ArgumentException($"Question with ID {question.Id} not found in the form.");

                existenceQuestion.ChangeText(question.Text, _currentUserService);
                existenceQuestion.ChangeType(question.Type, _currentUserService);
                existenceQuestion.ChangeOrderNumber(i + 1, _currentUserService);
            }
        }
    }
}
