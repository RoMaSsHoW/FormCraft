﻿using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
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

            await ChangeFormAsync(request);

            await ChangeQuestionsAsync(request);

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(UpdateFormWithQuestionCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            if (!_topicExisteceChecker.IsExist(request.NewTemplateInformation.TopicName))
                throw new ArgumentException("Topic name not exist");

            if (request.NewTemplateInformation.Questions.Count() <= 0)
                throw new ArgumentException("Question list cannot be null");
        }

        private async Task<List<Tag>> GetExistenceAndCreateNewTags(IEnumerable<string> tagNames)
        {
            var allTags = new List<Tag>();
            var newTags = new List<Tag>();

            foreach (var tagName in tagNames)
            {
                var existenseTag = await _tagRepository.FindByNameAsync(tagName);

                if (existenseTag != null)
                    allTags.Add(existenseTag);
                else
                    newTags.Add(new Tag(tagName));
            }

            if (newTags.Any())
            {
                await _tagRepository.CreateAsync(newTags);
                allTags.AddRange(newTags);
            }

            return allTags;
        }

        private async Task ChangeFormAsync(UpdateFormWithQuestionCommand request)
        {
            var existenceTemplate = await _formRepository.FindByIdAsync(request.NewTemplateInformation.Id);

            existenceTemplate.ChangeTitle(request.NewTemplateInformation.Title, _currentUserService);
            existenceTemplate.ChangeDescription(request.NewTemplateInformation.Description, _currentUserService);
            existenceTemplate.ChangeTopic(request.NewTemplateInformation.TopicName, _topicExisteceChecker, _currentUserService);
            existenceTemplate.ChangeVisibility(request.NewTemplateInformation.IsPublic, _currentUserService);

            var tags = await GetExistenceAndCreateNewTags(request.NewTemplateInformation.Tags);

            if (tags.Any())
                foreach (var tag in tags)
                    existenceTemplate.AddTag(tag, _currentUserService);

        }

        private async Task ChangeQuestionsAsync(UpdateFormWithQuestionCommand request)
        {
            for (var i = 0; i < request.NewTemplateInformation.Questions.Count; i++)
            {
                var question = request.NewTemplateInformation.Questions[i];
                var existenceQuestion = await _questionRepository.FindByIdAsync((Guid)question.Id!);
                existenceQuestion.ChangeText(question.Text, _currentUserService);
                existenceQuestion.ChangeType(question.Type, _currentUserService);
                existenceQuestion.ChangeOrderNumber(i + 1, _currentUserService);
            }
        }
    }
}
