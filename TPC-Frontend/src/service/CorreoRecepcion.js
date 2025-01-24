import instance from "../apis/app";

class CorreoRecepcionService {

    list = () => instance.get('/API/ControladorCorreo');


}

const CorreoRecepcion = new CorreoRecepcionService();
export default CorreoRecepcion;