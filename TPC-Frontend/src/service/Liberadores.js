import instance from "../apis/app";

class LiberadoresService {

    list = () => instance.get('/API/ControladorLiberadores');
    post = (data) => instance.post('/API/ControladorLiberadores', data);
    update = (id, data) => instance.put(`/API/ControladorLiberadores/${id}`, data);
}

const Liberadores = new LiberadoresService();
export default Liberadores;