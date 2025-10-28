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
        // PUBLIKA AUTO-IMPLEMENTERADE EGENSKAPER
        public required string UserName { get; set; } 
        public required string Password { get; set; } 
        public required string EmailAddress { get; set; } 
        public required string Country { get; set; }
        public string? PinCode { get; set; } // used by ChangePassword in UserManager

        // used by ChangePassword in UserManager

        // Egenskap för "hela" användarens info 
        public User() { }

        // KONSTRUKTOR
        public User(string username, string password, string email, string country)
        {
            UserName = username;
            Password = password;
            EmailAddress = email;
            Country = country;
        }
    }
}