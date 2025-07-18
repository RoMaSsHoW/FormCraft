using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands.FormWithQuestionCommands
{
    public class DeleteQuestionsOrTagsFromFormCommandHandler : ICommandHandler<DeleteQuestionsOrTagsFromFormCommand>
    {
        private readonly IFormRepository _formRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionsOrTagsFromFormCommandHandler(
            IFormRepository formRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _formRepository = formRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteQuestionsOrTagsFromFormCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var form = await _formRepository.FindByIdAsync(request.FormId);

            if (request.QuestionIds.Any())
            {
                foreach (var questionId in request.QuestionIds)
                {
                    form.RemoveQuestion(questionId, _currentUserService);
                }
                form.SetLastModifiedNow(_currentUserService);
            }

            if (request.TagIds.Any())
            {
                foreach (var tagId in request.TagIds)
                {
                    form.RemoveTag(tagId, _currentUserService);
                }
                form.SetLastModifiedNow(_currentUserService);
            }

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(DeleteQuestionsOrTagsFromFormCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            if (request.FormId == Guid.Empty)
                throw new ArgumentException("Form ID cannot be empty");
        }
    }
}
