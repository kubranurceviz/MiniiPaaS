using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Application.Queries;
using MiniiPaaS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniiPaaS.Application.Commands.CRM
{
    public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, List<Customer>>
    {
        private readonly IApplicationDbContext _db;

        public GetCustomersHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Customer>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _db.Customers
                .Where(c => c.CompanyId == request.CompanyId)
                .ToListAsync(cancellationToken);
        }
    }
}
