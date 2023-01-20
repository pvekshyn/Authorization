using Common.Application.Dependencies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Common.Infrastructure
{
    internal class CurrentContext : ICurrentContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var identity = _httpContextAccessor?.HttpContext?.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    return Guid.Parse(identity.FindFirst("client_sub").Value);
                }
                throw new Exception("Not authenticated");
            }
        }
    }
}
