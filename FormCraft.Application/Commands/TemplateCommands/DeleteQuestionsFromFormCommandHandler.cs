using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Application.Commands.Template
{
    public class DeleteQuestionsFromFormCommandHandler : ICommandHandler<DeleteQuestionsFromFormCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionsFromFormCommandHandler(
            IQuestionRepository questionRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteQuestionsFromFormCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var userDetails = GetUserDetails();

            var questionsToDelete = await GetQuestionsToDeleteAsync(request.QuestionIds, userDetails);

            if (questionsToDelete.Any())
            {
                _questionRepository.Remove(questionsToDelete);
                await _unitOfWork.CommitAsync();
            }

        }

        private void ValidateRequest(DeleteQuestionsFromFormCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            if (!request.QuestionIds.Any())
                throw new ArgumentException("Form list cannot be null");
        }

        private (Guid UserId, Role UserRole) GetUserDetails()
        {
            var userId = _currentUserService.GetUserId()!;
            var userRole = Role.FromName<Role>(_currentUserService.GetRole()!);

            return ((Guid)userId, userRole);
        }

        private async Task<IEnumerable<Question>> GetQuestionsToDeleteAsync(IEnumerable<Guid> questionIds, (Guid UserId, Role UserRole) userDetails)
        {
            var questions = await _questionRepository.FindQuestionsByIdAsync(questionIds);
            var questionsToDelete = new List<Question>();
            foreach (var question in questions)
            {
                if (question.AuthorId == userDetails.UserId || userDetails.UserRole == Role.Admin)
                {
                    questionsToDelete.Add(question);
                }
            }
            return questionsToDelete;
        }
    }
}
