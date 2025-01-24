using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatosTPC
{
    public class Liberadores
    {
        [Key]
        public int Id_Liberador { get; set; }
        public int Id_Usuario {  get; set; }    
        public string Nombre_Usuario { get; set; }
        public int Id_Departamento { get; set; }
        public string Nombre_Departamento { get; set; }
        public string? Correo { get; set; }
    }
}
