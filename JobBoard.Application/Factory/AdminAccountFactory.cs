using JobBoard.Application.Factory.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.AdminAccount;

namespace JobBoard.Application.Factory
{
    public class AdminAccountFactory : IAdminAccountFactory
    {
        public AdminAccount Create(AddAdminAccountRequest request, string passwordHash)
            => new AdminAccount
            {
                Login = request.Login,
                PasswordHash = passwordHash
            };
    }
}
