using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Tre_Monti.Models
{
    public class ServizioAggiuntivo
    {
        [Key]
        public int NumeroPrenotazione { get; set; }
        public DateTime? DataServizio { get; set; }
        public string TipoServizio { get; set; }
        public int Quantita { get; set; }
        public decimal Prezzo { get; set; }
    }
}