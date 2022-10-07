using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class User : AuditableEntity
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public List<Address> Addresses { get; set; }
        public Cart Cart { get; set; }
        public UserRole Role { get; set; }
        public string PasswordHash { get; set; }
    }
}
