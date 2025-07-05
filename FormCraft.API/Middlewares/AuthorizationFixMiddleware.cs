namespace FormCraft.API.Middlewares
{
    public class AuthorizationFixMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationFixMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                if (!token.ToString().StartsWith("Bearer "))
                {
                    context.Request.Headers["Authorization"] = "Bearer " + token.ToString();
                }
            }

            await _next(context);
        }
    }
}
