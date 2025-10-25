using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    public class User
    {
        // Auto-implementerade PUBLIKA EGENSKAPER som definierar en enskild användare 
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string DisplayName { get; set; }
        public required string EmailAddress { get; set; }
        public required string Role { get; set; } // Skulle kunna sätta ett default value (Member) och sedan kunna tilldela andra 
                                                  // ...roller vid specialtillfällen (Super Member, Administrator)

        // Lista med fördefinierade alternativ för land 
        public List<string> Countries { get; set; } = new List<string> { "Sweden", "Norway", "Denmark", "Finland", "New Zeeland", "Germany", "United Kingdom", "Other" };
    }
}