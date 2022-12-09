using Authorization.Infrastructure.DataAccess.Read;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Host.OData.Controllers
{
    [ODataAttributeRouting]
    public class AuthorizationODataController : ODataController
    {
        private readonly IAuthorizationDbContext _dbContext;

        public AuthorizationODataController(IAuthorizationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [EnableQuery]
        [HttpGet("odata/roles")]
        public IQueryable<Domain.Role> Get()
        {
            return _dbContext.Roles.AsNoTracking();
        }
    }
}