using MiniiPaaS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniiPaaS.Domain.Entities
{
 
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        // New properties for email confirmation
        public bool EmailConfirmed { get; set; }
        public string EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationTokenExpiry { get; set; }

        // Properties for lockout
        public int AccessFailedCount { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}
