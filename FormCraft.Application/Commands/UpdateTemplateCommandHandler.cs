using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Application.Commands
{
    public class UpdateTemplateCommandHandler : ICommandHandler<UpdateTemplateCommand>
    {
        private readonly IFormRepository _formRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITopicExistenceChecker _topicExisteceChecker;
        private readonly IUserRoleChecker _userRoleChecker;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTemplateCommandHandler(
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

        public async Task Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {

            ValidateRequest(request);
            var existenceTemplate = await _formRepository.FindByIdAsync(request.newTemplateInformation.Id);

            var user = existenceTemplate.AuthorId; // ВРЕМЕННО

            existenceTemplate.ChangeTitle(request.newTemplateInformation.Title, user, _userRoleChecker);
            existenceTemplate.ChangeDescription(request.newTemplateInformation.Description, user, _userRoleChecker);
            existenceTemplate.ChangeTopic(request.newTemplateInformation.TopicName, _topicExisteceChecker, user, _userRoleChecker);
            existenceTemplate.ChangeVisibility(request.newTemplateInformation.IsPublic, user, _userRoleChecker);

            



            throw new NotImplementedException();
        }

        private void ValidateRequest(UpdateTemplateCommand request)
        {
            if (!_topicExisteceChecker.IsExist(request.newTemplateInformation.TopicName))
                throw new ArgumentException("Topic name not exist");

            if (request.newTemplateInformation.Questions.Count() <= 0)
                throw new ArgumentException("Question list cannot be null");
        }
    }
}
