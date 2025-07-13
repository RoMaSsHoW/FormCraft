using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Application.Commands.Template
{
    public class AddQuestionsToFormCommandHandler : ICommandHandler<AddQuestionsToFormCommand>
    {
        private readonly IFormRepository _formRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public AddQuestionsToFormCommandHandler(
            IFormRepository formRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _formRepository = formRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddQuestionsToFormCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var form = await _formRepository.FindByIdAsync(request.FormId);

            foreach (var question in request.Questions)
                form.AddQuestion(question.Text, question.Type, _currentUserService);

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(AddQuestionsToFormCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            if (request.FormId == Guid.Empty)
                throw new ArgumentException("Form ID cannot be empty");

            if (!request.Questions.Any())
                throw new ArgumentException("Question list cannot be null");
        }
    }
}
