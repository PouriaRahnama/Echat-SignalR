using Echat.Domain.Context;

namespace Echat.Application.Services.Roles
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(EchatContext context) : base(context)
        {
        }
    }
}