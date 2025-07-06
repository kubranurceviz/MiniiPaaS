using MediatR;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Domain.Entities;
using MiniiPaaS.Application.Commands.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniiPaaS.Application.Commands.CRM
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly IApplicationDbContext _db;

        public CreateCustomerHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                CompanyId = request.CompanyId
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync(cancellationToken);
            return customer.Id;
        }
    }

}
