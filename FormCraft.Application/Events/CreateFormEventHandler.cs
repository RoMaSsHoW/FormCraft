using FormCraft.Application.Common.Messaging;
using FormCraft.Domain.Aggregates.FormAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using FormCraft.Domain.Events;
using MassTransit;

namespace FormCraft.Application.Events
{
    public class CreateFormEventHandler : IDomainEventHandler<CreateForm>
    {
        private readonly IFormRepository _formRepository;
        private readonly IUserRepository _userRepository;

        public CreateFormEventHandler(
            IFormRepository formRepository,
            IUserRepository userRepository)
        {
            _formRepository = formRepository;
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<CreateForm> context)
        {
            var form = await _formRepository.FindByIdAsync(context.Message.FormId);
            var user = await _userRepository.FindById(form.AuthorId);
            if (user.Role == Role.Admin)
            {
                Console.WriteLine($"\n\n\n\n\n{new string('-', 20)}\n");
                Console.WriteLine("Consumer работает!");
                Console.WriteLine($"Форма: {form.Title}");
                Console.WriteLine($"Автор: {user.Name}");
                Console.WriteLine($"Роль автора: {user.Role}");
                Console.WriteLine($"\n{new string('-', 20)}\n");
            }
        }
    }
}
