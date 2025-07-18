using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Models.RequestModels;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands.FormWithAnswerCommands
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

            foreach (var answerToDelete in request.AnswersToDelete)
                await DeletingAnswers(answerToDelete);

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(DeleteAnswersFromQoestionsCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");
        }

        private async Task DeletingAnswers(AnswersToDeleteRequestModel request)
        {
            var question = await _questionRepository.FindByIdAsync(request.QuestionId);
            if (question == null)
                throw new KeyNotFoundException($"Question with ID {request.QuestionId} not found.");

            foreach (var answerId in request.AnswerIds)
                question.RemoveAnswer(answerId, _currentUserService);
        }
    }
}
