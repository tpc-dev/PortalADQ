using System.ComponentModel.DataAnnotations;


namespace ClasesBaseDatosTPC
{
    public class Usuario
    {
        [Key]
        public int Id_Usuario { get; set; }
        public string? Nombre_Usuario { get; set; }
        public string? Apellido_paterno { get; set; }
        public string? Rut_Usuario { get; set; }
        public string? Apellido_materno { get; set; }
        public string? Correo_Usuario { get; set; }
        public string? Contraseña_Usuario { get; set; }
        public bool? En_Vacaciones {  get; set; }
        public bool Tipo_Liberador {  get; set; }
        public bool Activado { get; set; }
        public bool Admin {  get; set; }
        public List<String> ListaDepartamento { get; set; }
        public List<int> ListaIdDep { get; set; }

        public int? CodigoMFA { get; set; }
        public string? Nombre_Completo { get; set; }
        public int Id_Departamento { get;set; }


    }
}
