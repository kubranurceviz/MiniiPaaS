using MediatR;
using MiniiPaaS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniiPaaS.Application.Queries
{
    public class GetCustomersQuery : IRequest<List<Customer>>
    {
        public int CompanyId { get; set; }
    }
}
