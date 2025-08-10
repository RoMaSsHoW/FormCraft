using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.ViewModels;

namespace FormCraft.Application.Commands.AuthCommands
{
    public record RefreshAccessTokenCommand(
        string RefreshToken) : ICommand<AuthResponse>;
}
