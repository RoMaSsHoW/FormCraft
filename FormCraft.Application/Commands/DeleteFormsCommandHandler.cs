using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Application.Services;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Application.Commands
{
    public class DeleteFormsCommandHandler : ICommandHandler<DeleteFormsCommand>
    {
        private readonly IFormRepository _formRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFormsCommandHandler(
            IFormRepository formRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _formRepository = formRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteFormsCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var userId = _currentUserService.GetUserId();
            var userRole = _currentUserService.GetRole();

            var forms = await _formRepository.FindFormsByIdAsync(request.FormIds);
            var allowedForms = new List<Form>();

            foreach (var form in forms)
            {
                if (form.AuthorId == userId || Role.FromName<Role>(userRole) == Role.Admin)
                {
                    allowedForms.Add(form);
                }
            }

            _formRepository.RemoveAsync(allowedForms);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private void ValidateRequest(DeleteFormsCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");
        }
    }
}
