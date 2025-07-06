using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniiPaaS.Application.Commands.CRM
{
    public class CreateCustomerCommand : IRequest<int>
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public int CompanyId { get; set; } // Token'dan alınabilir
    }
}
