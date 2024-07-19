using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Command
{
    public class GetBusinessCountCommand : GetBusinessRequest, IGetCountCommand
    {
    }

    public class GetBusinessCountHandler : GetCountHandler<BusinessModel, GetBusinessRequest, GetBusinessCountCommand>
    {
        public GetBusinessCountHandler(IBusinessRepository repository) : base(repository)
        {
            
        }
    }
}
