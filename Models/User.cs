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
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }        
        // Lista med fördefinierade alternativ för land 
        public List<string> Countries { get; set; } = new List<string> { "Sweden", "Norway", "Denmark", "Finland", "New Zeeland", "Germany", "United Kingdom", "Other" };

        // KONSTRUKTOR - måste ha konstruktor för att kunna implementera barnklassen AdminUser? 
        public User(string UserName, string DisplayName, string EmailAddress, string Role, string PassWord)
        {
            this.Username = UserName;
            this.DisplayName = DisplayName;
            this.EmailAddress = EmailAddress; 
            this.Role = Role;
            this.Password = PassWord;
        }
        // METOD för att validera e-postadress vid skapande av användare 
        public (bool success, string message) ValidateEmailAddress(string email)
        {
            //string message = isValidEmail ? "E-postadressen är giltig" : "E-postadressen är ogiltig. Försök igen.";
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

        // METOD för att validera lösenord vid skapande av användare och nytt lösenord 
        public (bool success, string message) ValidatePassWord(string password)
        {
            // villkor för att validera nytt lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            bool IsValidPassWord = (Password.Length > 4 && Password.Length < 8 && Password.Any(char.IsUpper) && Password.Any(char.IsLower)
                    && Password.Any(char.IsDigit) && Password.Contains(specialCharacters)) ? true : false; // Ternary sats istället för IF-sats
            string message = IsValidPassWord ? "Lösenordet har sparats" : "Lösenordet uppfyller inte kraven";
            if (IsValidPassWord)
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
            bool IsPassWordMatch = (Password == passwordRepeat) ? true : false; // Ternary sats istället för IF-sats
            string message = IsPassWordMatch ? "Lösenordet har sparats" : "Lösenorden matchar inte varandra";
            if (IsPassWordMatch)
            {
                Password = passwordRepeat;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }
    }
}
