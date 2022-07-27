using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnigisTest.Models
{
    public class Raza
    {
        [Key]
        public int RazaID { get; set; }
        [Column(TypeName = "varchar")]
        [MaxLength(10)]
        public string? NombreRaza { get; set; }    
    }
}
