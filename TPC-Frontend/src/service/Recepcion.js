import instance from "../apis/app";

class RecepcionService {

    get = () => instance.get('/API/ControladorRecepcion');
}

const Recepcion = new RecepcionService();
export default Recepcion;