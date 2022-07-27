using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnigisTest.Models
{
    public class FotoRaza
    {
        [Key]
        public int FotoRazaID { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(70)]
        public string? RutaImagen { get; set; }
        public Raza? Raza{ get; set; }
    }
}
