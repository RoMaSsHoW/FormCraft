using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Application.Commands
{
    public class AddQuestionsToFormCommandHandler : ICommandHandler<AddQuestionsToFormCommand>
    {
        private readonly IFormRepository _formRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public AddQuestionsToFormCommandHandler(
            IFormRepository formRepository,
            IQuestionRepository questionRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _formRepository = formRepository;
            _questionRepository = questionRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddQuestionsToFormCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var userDetails = GetUserDetails();

            var form = await GetAndValidateForm(request.FormId, userDetails);

            var newQuestions = GenerateNewQuestions(request, form, userDetails.UserId);

            if (newQuestions.Any())
            {
                await _questionRepository.CreateAsync(newQuestions);
                await _unitOfWork.CommitAsync();
            }
        }

        private void ValidateRequest(AddQuestionsToFormCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            if (!request.Questions.Any())
                throw new ArgumentException("Question list cannot be null");
        }

        private (Guid UserId, Role UserRole) GetUserDetails()
        {
            var userId = _currentUserService.GetUserId()!;
            var userRole = Role.FromName<Role>(_currentUserService.GetRole()!);

            return ((Guid)userId, userRole);
        }

        private async Task<Form> GetAndValidateForm(Guid formId, (Guid UserId, Role UserRole) userDetails)
        {
            var form = await _formRepository.FindByIdAsync(formId);
            if (form == null)
                throw new ArgumentException("Form not found");

            if (!(form.AuthorId == userDetails.UserId || userDetails.UserRole == Role.Admin))
                throw new ArgumentException("User not author or admin");

            return form;
        }

        private IEnumerable<Question> GenerateNewQuestions(AddQuestionsToFormCommand request, Form form, Guid userId)
        {
            var newQuestions = new List<Question>();
            var existingQuestions = form.Questions;
            var lastOrderNumber = existingQuestions.Any() ? existingQuestions.Max(q => q.OrderNumber) : 0;

            foreach (var question in request.Questions)
            {
                var type = QuestionType.FromName<QuestionType>(question.Type);

                if (existingQuestions.Any(q => q.Text == question.Text && q.Type == type))
                    continue;

                var newQuestion = Question.Create(form.Id, userId, question.Text, question.Type, lastOrderNumber + 1);

                newQuestions.Add(newQuestion);
            }

            return newQuestions;
        }
    }
}
