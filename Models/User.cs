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
        public string SecurityQuestion = "Vilka är alfabetets tre första bokstäver?"; //{ get; set; } // textfråga på låtsas
        public string SecurityAnswer = "abc"; //{ get; set; } 

        // En enklare flagga som visar om användaren är admin eller ej.
        public bool IsAdmin { get; set; } = false;

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
        // Konstruktor för ny användare
        public User(string username, string newPassword, string repeatPassword, string email, string country)
        {
            UserName = username;
            Password = newPassword;
            EmailAddress = email;
            Country = country;
        }
    }
}