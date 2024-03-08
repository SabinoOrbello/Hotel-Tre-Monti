using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Tre_Monti.Models
{
    public class Camera
    {
        [Key]
        public int Numero { get; set; }
        public string Descrizione { get; set; }
        public string Tipologia { get; set; }
    }
}