using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.ViewModels
{
    class UserDetailsViewModel
    {

        public required string DisplayName { get; set; }
        public required string Role { get; set; } // Skulle kunna sätta ett default value (Member) och sedan kunna tilldela andra 
                                                  // ...roller vid specialtillfällen (Super Member, Administrator)

    }
}
