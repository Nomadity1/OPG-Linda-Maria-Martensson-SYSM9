using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    public class User
    {
        // PUBLIKA AUTO-IMPLEMENTERADE EGENSKAPER
        public required string UserName { get; set; } 
        public required string Password { get; set; }
        public required string PasswordRepeat { get; set; }
        public required string Email { get; set; } 
        public required string Country { get; set; }
        public User()
        {
        }
        public User(string userName, string password, string email, string country)
        {
            UserName = userName;
            Password = password;
            PasswordRepeat = password; // Ska tilldelas om inget värde
            Email = email;
            Country = country;
        }
        public User(string userName, string password, string passwordRepeat, string email, string country)
        {
            UserName = userName;
            Password = password;
            PasswordRepeat = passwordRepeat;
            Email = email;
            Country = country;
        }
    }
}