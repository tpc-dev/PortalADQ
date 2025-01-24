import instance from "../apis/app";

class BienServicioService {

    get = () => instance.get('/API/ControladorBienServicio');
    create = (data) => instance.post('/API/ControladorBienServicio', data);
    update = (id, data) => instance.put(`/API/ControladorBienServicio/${id}`, data);
    delete = (id) => instance.delete(`/API/ControladorBienServicio/${id}`)

}

const BienServicio = new BienServicioService();
export default BienServicio;