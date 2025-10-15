using System.Security.Claims;

namespace myapp
{
    public interface IUserContext
    {
        int UserId { get; }
        string? UserName { get; } // Allow UserName to be nullable
        bool IsAuthenticated { get; }
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpContext? HttpContext => _httpContextAccessor.HttpContext;

        public int UserId
        {
            get
            {
                var userIdClaim = HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.TryParse(userIdClaim, out var userId) ? userId : 0; // Default to 0 if not found
            }
        }

        public string? UserName => HttpContext?.User.Identity?.Name;

        public bool IsAuthenticated => HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
