
namespace BaseDatosTPC
{
    /// <summary>
    /// clase que guarda los archivos
    /// </summary>
    public class Archivo
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        public int Id_Archivo { get; set; }

        /// <summary>
        /// Guarda el archivo
        /// </summary>
        public byte[]? ArchivoDoc { get; set; }
        public string? NombreDoc { get; set; }
    }
}
