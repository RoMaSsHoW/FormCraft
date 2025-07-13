using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Answers;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Application.Commands.FormCommands
{
    public class SetAnswerToQuestionCommandHandler : ICommandHandler<SetAnswerToQuestionCommand>
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public SetAnswerToQuestionCommandHandler(
            IAnswerRepository answerRepository,
            IQuestionRepository questionRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SetAnswerToQuestionCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var question = await _questionRepository.FindByIdAsync(request.AnswerRequest.QuestionId);
            var userDetails = GetUserDetails();

            Answer answer = null!;

            if (question.Type == QuestionType.Text)
            {
                answer = TextAnswer.Create(
                    userDetails.UserId,
                    request.AnswerRequest.QuestionId,
                    request.AnswerRequest.AnswerValue);
            }
            else if (question.Type == QuestionType.Number)
            {
                if (int.TryParse(request.AnswerRequest.AnswerValue, out var numberValue))
                {
                    answer = NumberAnswer.Create(
                        userDetails.UserId,
                        request.AnswerRequest.QuestionId,
                        numberValue);
                }
                else
                {
                    throw new ArgumentException("Invalid number format for AnswerValue");
                }
            }
            else if (question.Type == QuestionType.Boolean)
            {
                if (bool.TryParse(request.AnswerRequest.AnswerValue, out var bolleanValue))
                {
                    answer = BooleanAnswer.Create(
                        userDetails.UserId,
                        request.AnswerRequest.QuestionId,
                        bolleanValue);
                }
                else
                {
                    throw new ArgumentException("Invalid bollean format for AnswerValue");
                }
            }
            else
            {
                throw new ArgumentException("Unsupported question type");
            }

            question.SetAnswer(answer);

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(SetAnswerToQuestionCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");
        }

        private (Guid UserId, Role UserRole) GetUserDetails()
        {
            var userId = _currentUserService.GetUserId()!;
            var userRole = Role.FromName<Role>(_currentUserService.GetRole()!);

            return ((Guid)userId, userRole);
        }
    }
}
