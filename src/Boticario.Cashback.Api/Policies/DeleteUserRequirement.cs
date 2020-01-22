using Microsoft.AspNetCore.Authorization;

namespace Boticario.Cashback.Api.Policies
{
    public class DeleteUserRequirement : IAuthorizationRequirement
    {
        public string RequiredPermission { get; }

        public DeleteUserRequirement(string requiredPermission)
        {
            RequiredPermission = requiredPermission;
        }
    }
}
