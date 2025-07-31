using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.RequestModels;
using FormCraft.Application.Models.ViewModels;

namespace FormCraft.Application.Commands.AuthCommands
{
    public record RegistrationCommand(
        RegisterRequest RegisterRequest) : ICommand<AuthResponse>;
}
