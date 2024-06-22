using JobBoard.Domain.Entity;
using JobBoard.Model.AdminAccount;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface IAdminAccountFactory
    {
        AdminAccount Create(AddAdminAccountRequest request, string passwordHash);
    }
}
