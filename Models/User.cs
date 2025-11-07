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
        public required string Country { get; set; }

        // OVERLOADANDE KONSTRUKTORER för olika sätt att skapa ett User-objekt
        public User()
        {
        }
        public User(string username, string password)
        {
            UserName = username;
            Password = password;
        }
        public User(string username, string password, string country)
        {
            UserName = username;
            Password = password;
            Country = country;
        }
    }
}