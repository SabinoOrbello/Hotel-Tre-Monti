using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Tre_Monti.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Il campo {0} è obbligatorio.")]
        [Display(Name = "Nome Utente")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Il campo {0} è obbligatorio.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Ricorda le credenziali")]
        public bool RememberMe { get; set; }
    }
}