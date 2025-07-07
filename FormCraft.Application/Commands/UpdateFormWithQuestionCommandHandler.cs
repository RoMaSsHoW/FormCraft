using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Application.Services;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands
{
    public class UpdateFormWithQuestionCommandHandler : ICommandHandler<UpdateFormWithQuestionCommand>
    {
        private readonly IFormRepository _formRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITopicExistenceChecker _topicExisteceChecker;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFormWithQuestionCommandHandler(
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

        public async Task Handle(UpdateFormWithQuestionCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var existenceTemplate = await _formRepository.FindByIdAsync(request.newTemplateInformation.Id);

            existenceTemplate.ChangeTitle(request.newTemplateInformation.Title, _currentUserService);
            existenceTemplate.ChangeDescription(request.newTemplateInformation.Description, _currentUserService);
            existenceTemplate.ChangeTopic(request.newTemplateInformation.TopicName, _topicExisteceChecker, _currentUserService);
            existenceTemplate.ChangeVisibility(request.newTemplateInformation.IsPublic, _currentUserService);

            var allQuestions = new List<Guid>();

            for (var i = 0; i < request.newTemplateInformation.Questions.Count; i++)
            {
                var question = request.newTemplateInformation.Questions[i];
                var existenceQuestion = await _questionRepository.FindByIdAsync((Guid)question.Id!);
                existenceQuestion.ChangeText(question.Text, _currentUserService);
                existenceQuestion.ChangeType(question.Type, _currentUserService);
                existenceQuestion.ChangeOrderNumber(i + 1, _currentUserService);
            }

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(UpdateFormWithQuestionCommand request)
        {
            if (!_topicExisteceChecker.IsExist(request.newTemplateInformation.TopicName))
                throw new ArgumentException("Topic name not exist");

            if (request.newTemplateInformation.Questions.Count() <= 0)
                throw new ArgumentException("Question list cannot be null");
        }
    }
}
