using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

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
            var existenceForm = await _formRepository.FindByIdAsync(request.FormId);

            var newQuestions = new List<Question>();

            foreach (var question in request.Questions)
            {
                if (existenceForm.Questions.Any(q => q.Text == question.Text
                && q.Type == QuestionType.FromName<QuestionType>(question.Type)))
                    continue;

                var lastOrderNumber = existenceForm.Questions.Any() ? existenceForm.Questions.Max(q => q.OrderNumber) : 0;

                newQuestions.Add(Question.Create(existenceForm.Id, existenceForm.AuthorId, question.Text, question.Type, lastOrderNumber + 1));
            }

            if (newQuestions.Any())
            {
                await _questionRepository.CreateAsync(newQuestions);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
