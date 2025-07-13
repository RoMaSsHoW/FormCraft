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
            ValidateRequest(request);

            var form = await ChangeFormAsync(request);

            ChangeQuestions(form, request);

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(UpdateFormWithQuestionCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            if (!string.IsNullOrWhiteSpace(request.TopicName))
                if (!_topicExisteceChecker.IsExist(request.TopicName))
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

        private async Task<Form> ChangeFormAsync(UpdateFormWithQuestionCommand request)
        {
            var existingForm = await _formRepository.FindByIdAsync(request.FormId);

            if (!string.IsNullOrWhiteSpace(request.Title))
                existingForm.ChangeTitle(request.Title, _currentUserService);

            if (!string.IsNullOrWhiteSpace(request.Description))
                existingForm.ChangeDescription(request.Description, _currentUserService);

            if (!string.IsNullOrWhiteSpace(request.TopicName))
                existingForm.ChangeTopic(request.TopicName, _topicExisteceChecker, _currentUserService);

            existingForm.ChangeVisibility(request.IsPublic, _currentUserService);

            existingForm.SetLastModifiedNow(_currentUserService);

            var tags = await GetOrCreateTagsAsync(request.Tags);

            if (tags.Any())
                foreach (var tag in tags)
                    existingForm.AddTag(tag, _currentUserService);

            return existingForm;
        }

        private void ChangeQuestions(Form form, UpdateFormWithQuestionCommand request)
        {
            for (var i = 0; i < request.Questions.Count(); i++)
            {
                var question = request.Questions.ToList()[i];
                var existenceQuestion = form.Questions.First(q => q.Id == question.Id);
                existenceQuestion.ChangeText(question.Text, _currentUserService);
                existenceQuestion.ChangeType(question.Type, _currentUserService);
                existenceQuestion.ChangeOrderNumber(i + 1, _currentUserService);
            }
        }
    }
}
