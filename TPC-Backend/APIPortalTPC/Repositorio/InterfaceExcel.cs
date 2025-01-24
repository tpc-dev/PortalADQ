using BaseDatosTPC;

namespace APIPortalTPC.Repositorio
{
    /// <summary>
    /// Interface que contiene todos los metodos para leer excel en especificos y generar un objeto correspondiente a su lectura
    /// </summary>
    public interface InterfaceExcel
    {
        public Task<Proveedores> LeerExcelProveedor(byte[] archivo);
        public Task<List<Proveedores>> LeerProveedores(byte[] archivo);
        //public Task<> LeerReporteSap(string filePath);
        public Task<List<CentroCosto>> LeerExcelCeCo(byte[] archivo);
        public Task<string> ActualizarOC(byte[] archivo);
        public Task<List<BienServicio>> LeerBienServicio(byte[] archivo);
        public Task<string>  LeerExcelOC(byte[] archivo);
        public Task<List<OrdenesEstadisticas>> LeerExcel(byte[] archivo);


    }
}
