using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands.FormCommands
{
    public class DeleteAnswersFromQoestionsCommandHandler : ICommandHandler<DeleteAnswersFromQoestionsCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAnswersFromQoestionsCommandHandler(
            IQuestionRepository questionRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteAnswersFromQoestionsCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            foreach (var answerToDeleteRequest in request.AnswersToDeleteRequest)
            {
                var question = await _questionRepository.FindByIdAsync(answerToDeleteRequest.QuestionId);
                if (question == null)
                    throw new ArgumentException($"Question with ID {answerToDeleteRequest.QuestionId} not found.");

                foreach (var answerId in answerToDeleteRequest.AnswerIds)
                {
                    question.RemoveAnswer(answerId, _currentUserService);
                }
            }

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(DeleteAnswersFromQoestionsCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");
        }
    }
}
