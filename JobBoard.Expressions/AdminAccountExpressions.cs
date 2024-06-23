using JobBoard.Domain.Entity;
using JobBoard.Model.AdminAccount;
using System.Linq.Expressions;

namespace JobBoard.Expressions
{
    public static class AdminAccountExpressions
    {
        public static Expression<Func<AdminAccount, AdminAccountModel>> Model { get; } = account => new AdminAccountModel
        {
            Id = account.Id,
            Login = account.Login,
        };
    }
}
