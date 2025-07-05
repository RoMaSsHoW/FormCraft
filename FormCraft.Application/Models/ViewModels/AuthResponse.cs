using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Application.Models.ViewModels
{
    public class AuthResponse
    {
        public AuthResponse() : this(string.Empty, string.Empty)
        {
        }

        public AuthResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }

        public string AccessToken { get; }

        public string RefreshToken { get; }
    }
}
