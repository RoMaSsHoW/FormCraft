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

            var question = await _questionRepository.FindByIdAsync(request.QuestionId);
            var userDetails = GetUserDetails();

            Answer answer;

            if (question.Type == QuestionType.Text)
            {
                answer = TextAnswer.Create(
                    userDetails.UserId,
                    request.QuestionId,
                    request.answerValue.ToString()!);
            }
            else if (question.Type == QuestionType.Number)
            {
                answer = NumberAnswer.Create(
                    userDetails.UserId,
                    request.QuestionId,
                    Convert.ToInt32(request.answerValue));
            }
            else if (question.Type == QuestionType.Boolean)
            {
                answer = BooleanAnswer.Create(
                    userDetails.UserId,
                    request.QuestionId,
                    Convert.ToBoolean(request.answerValue));
            }
            else
            {
                throw new ArgumentException("Unsupported question type");
            }

            await _answerRepository.AddAsync(answer);
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
