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
        public required string Role { get; set; }

        // Lista med fördefinierade alternativ för land 
        public List<string> Countries { get; set; } = new List<string> { "Sweden", "Norway", "Denmark", "Finland", "New Zeeland", "Germany", "United Kingdom", "Other" };
        
        // METOD för att validera användarnamn vid skapande av användare 
        public (bool success, string message) ValidateUsername(string username)
        {
            bool IsValidUsername = (username.Length > 3 && username.Length < 9) ? true : false; // Ternary sats istället för IF-sats för att kontrollera om e-postadressen är giltig
            string message = IsValidUsername ? "Användarnamnet har sparats" : "Användarnamnet ska vara mellan 4 och 8 bokstäver eller tecken långt";
            if (IsValidUsername)
            {
                string UserName = username;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }

        // METOD för att validera lösenord vid skapande av användare 
        public (bool success, string message) ValidatePassword(string password)
        {
            // villkor för att validera nytt lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            bool IsValidPassword = (Password.Length > 3 && Password.Length < 9 && Password.Any(char.IsUpper) && Password.Any(char.IsLower)
                    && Password.Any(char.IsDigit) && Password.Contains(specialCharacters)) ? true : false; // Ternary sats istället för IF-sats
            string message = IsValidPassword ? "Lösenordet har sparats" : "Lösenordet ska innehålla minst 4 och max 8 bokstäver inklusive ett specialtecken och en siffra";
            if (IsValidPassword)
            {
                Password = password;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }

        // METOD för att validera lösenord vid byte av nytt lösenord
        public (bool success, string message) ValidateRepeatedPassWord(string passwordRepeat)
        {
            // villkor för lösenordsinmatning
            bool IsPasswordMatch = (Password == passwordRepeat) ? true : false; // Ternary sats istället för IF-sats
            string message = IsPasswordMatch ? "Lösenordet har sparats" : "Lösenorden matchar inte varandra";
            if (IsPasswordMatch)
            {
                Password = passwordRepeat;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }

        // METOD för att validera e-postadress vid skapande av användare 
        public (bool success, string message) ValidateEmailAddress(string email)
        {
            bool IsValidEmailAddress = (email.Contains("@") && email.IndexOf('.') > email.IndexOf('@')) ? true : false; // Ternary sats istället för IF-sats för att kontrollera om e-postadressen är giltig
            string message = IsValidEmailAddress ? "E-postadressen har sparats" : "E-postadressen är ogiltig. Försök igen.";
            if (IsValidEmailAddress)
            {
                EmailAddress = email;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }
    }
}