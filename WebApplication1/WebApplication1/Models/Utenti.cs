using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Utenti
    {
        public int IdUtente { get; set; }
        [Required]
        [Display(Name = "Nome Utente")]
        public string NomeUtente { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password {  get; set; }
        public string Ruolo {  get; set; }
    }
}