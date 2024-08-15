using JobBoard.Domain.Entity;
using JobBoard.Model.EmployeeResume;
using System.Linq.Expressions;

namespace JobBoard.Expressions
{
    public static class EmployeeResumeExpressions
    {
        public static Expression<Func<EmployeeResume, EmployeeResumeModel>> Model { get; } = resume => new EmployeeResumeModel
        {
            Id = resume.Id,
            Name = resume.Name,
        };
    }
}
