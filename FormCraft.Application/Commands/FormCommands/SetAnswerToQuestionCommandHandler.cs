using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands.FormCommands
{
    public class SetAnswerToQuestionCommandHandler : ICommandHandler<SetAnswerToQuestionCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public SetAnswerToQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SetAnswerToQuestionCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            Guid userId = (Guid)_currentUserService.GetUserId()!;

            var question = await _questionRepository.FindByIdAsync(request.AnswerRequest.QuestionId);

            question.SetAnswer(request.AnswerRequest.AnswerValue, userId);

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(SetAnswerToQuestionCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");
        }
    }
}
