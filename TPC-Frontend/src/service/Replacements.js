import instance from "../apis/app";

class ReplacementService {

    list = () => instance.get('/API/ControladorReemplazos');
    create = (data) => instance.post('/API/ControladorReemplazos', data);
    update = (id, data) => instance.put(`/API/ControladorReemplazos/${id}`, data);
    cancel = (id) => instance.delete(`/API/ControladorReemplazos/${id}`);

}

const Reemplazos = new ReplacementService();
export default Reemplazos;