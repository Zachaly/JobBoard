using JobBoard.Domain.Entity;
using JobBoard.Model.Business;
using System.Linq.Expressions;

namespace JobBoard.Expressions
{
    public static class BusinessExpressions
    {
        public static Expression<Func<Business, BusinessModel>> Model { get; } = business => new BusinessModel
        {
            Id = business.Id,
            Name = business.Name,
        };
    }
}
