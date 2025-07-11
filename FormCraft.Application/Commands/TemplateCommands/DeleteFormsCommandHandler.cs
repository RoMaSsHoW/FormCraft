using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Application.Commands.Template
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

            var userDetails = GetUserDetails();

            var formsToDelete = await GetFormsToDeleteAsync(request.FormIds, userDetails);

            if (formsToDelete.Any())
            {
                _formRepository.Remove(formsToDelete);
                await _unitOfWork.CommitAsync();
            }
        }

        private void ValidateRequest(DeleteFormsCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            if (!request.FormIds.Any())
                throw new ArgumentException("Form list cannot be null");
        }

        private (Guid UserId, Role UserRole) GetUserDetails()
        {
            var userId = _currentUserService.GetUserId()!;
            var userRole = Role.FromName<Role>(_currentUserService.GetRole()!);

            return ((Guid)userId, userRole);
        }

        private async Task<IEnumerable<Form>> GetFormsToDeleteAsync(IEnumerable<Guid> formIds, (Guid UserId, Role UserRole) userDetails)
        {
            var forms = await _formRepository.FindFormsByIdAsync(formIds);
            var formsToDelete = new List<Form>();

            foreach (var form in forms)
            {
                if (form.AuthorId == userDetails.UserId || userDetails.UserRole == Role.Admin)
                {
                    formsToDelete.Add(form);
                }
            }

            return formsToDelete;
        }
    }
}
