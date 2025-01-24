using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{

    /// <summary>
    /// 
    /// </summary>
    public class Proveedores
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key]
        public int ID_Proveedores {  get; set; }

        public string? Rut_Proveedor {  get; set; }
        
        public string? Razon_Social { get; set; }

        public string? Nombre_Fantasia { get; set; }

        public string? ID_Bien_Servicio { get; set; }

        public string? Direccion { get; set; }
        
        public string? Comuna { get; set; }
        
        public string? Correo_Proveedor { get; set; }

        public string? Telefono_Proveedor { get; set; }

        public string? Cargo_Representante { get; set; }    

        public string? Nombre_Representante { get; set; }

        public string? Email_Representante { get; set; }

        public bool? Estado { get; set; }

        public string? N_Cuenta { get; set; }

        public string? Banco { get; set; }

        public string? Swift { get; set; }
  


    }
}
