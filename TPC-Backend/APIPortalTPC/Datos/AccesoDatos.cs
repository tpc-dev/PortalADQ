namespace APIPortalTPC.Datos
{
    public class AccesoDatos
    {
        private string connectSQL;

        public string ConexionDatosSQL {
            get => connectSQL;
        }
        public AccesoDatos (string cSQL)
        {
            connectSQL = cSQL;
        }
    }
}

