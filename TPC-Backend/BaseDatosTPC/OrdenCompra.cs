using System.ComponentModel.DataAnnotations;


namespace BaseDatosTPC
{
    /// <summary>
    /// Clase que guarda los datos de las ordenes de compras
    /// </summary>
    public class OrdenCompra
    {
        /// <summary>
        /// Identificador unico de la relacion
        /// </summary>
        [Key]
        public int Id_Orden_Compra { get; set; }
        /// <summary>
        /// Codigo de la orden de compra
        /// </summary>
        public long? Numero_OC { get; set; }
        /// <summary>
        /// Id del ticket asociado a la orden de compra
        /// </summary>
        public int? Id_Ticket { get; set; }

        /// <summary>
        /// fecha que la orden de compra es liberada
        /// </summary>
        public DateTime? Fecha_Recepcion { get; set; }
        public string? Texto { get; set; }
        public bool? IsCiclica { get; set; }

        /// <summary>
        /// Orden de posicionamiento entre varias ordenes de compras relacionadas
        /// </summary>
        public string? posicion { get; set; }
        /// <summary>
        /// Cantidad de materiales
        /// </summary>
        public int? Cantidad {get; set;}
        /// <summary>
        /// Tipo de moneda
        /// </summary>
        public string? Mon {get; set;}
        //Precio unitario
        public decimal? PrcNeto {get; set;}
        /// <summary>
        /// Nombre del proveedor
        /// </summary>
        public string? Proveedor { get; set; }
        public int? IdP { get; set; }
        /// <summary>
        /// Codigo del material
        /// </summary>
        public long? Material { get; set; }
        /// <summary>
        /// Valor del total
        /// </summary>
        public decimal? ValorNeto { get; set; }
        /// <summary>
        /// Es para ver si fue recepcionado la orden de compra
        /// </summary>
        public bool? Recepcion { get; set; }
        public bool? Estado_OC { get; set; }

    }
}
