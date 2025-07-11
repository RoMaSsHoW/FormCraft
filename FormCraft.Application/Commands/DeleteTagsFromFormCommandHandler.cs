using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;

namespace FormCraft.Application.Commands
{
    public class DeleteTagsFromFormCommandHandler : ICommandHandler<DeleteTagsFromFormCommand>
    {
        private readonly IFormTagRepository _formTagRepository;
        private readonly IFormRepository _formRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTagsFromFormCommandHandler(
            IFormTagRepository formTagRepository,
            IFormRepository formRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _formTagRepository = formTagRepository;
            _formRepository = formRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteTagsFromFormCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var userDetails = GetUserDetails();

            await GetTagsToDeleteAsync(request.FormId, request.TagIds, userDetails);

            await _unitOfWork.CommitAsync();
        }

        private void ValidateRequest(DeleteTagsFromFormCommand request)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            if (!request.TagIds.Any())
                throw new ArgumentException("Tag list cannot be null");
        }

        private (Guid UserId, Role UserRole) GetUserDetails()
        {
            var userId = _currentUserService.GetUserId()!;
            var userRole = Role.FromName<Role>(_currentUserService.GetRole()!);

            return ((Guid)userId, userRole);
        }

        private async Task GetTagsToDeleteAsync(Guid formId, IEnumerable<Guid> tagIds, (Guid UserId, Role UserRole) userDetails)
        {
            var form = await _formRepository.FindByIdAsync(formId);
            var formTags = await _formTagRepository.FindFormTagsByTagIdsAsync(tagIds);

            if (form.AuthorId == userDetails.UserId || userDetails.UserRole == Role.Admin)
            {
                _formTagRepository.Remove(formTags);
            }
        }
    }
}
