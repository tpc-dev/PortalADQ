import instance from "../apis/app";

class ArchivoService {

    post = (data) => instance.post('/API/ControladorArchivo', data);
}

const Archivo = new ArchivoService();
export default Archivo;