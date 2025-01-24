import instance from "../apis/app";

class BienCentroCostoService {

    list = () => instance.get('/API/ControladorCentroCosto');
    create = (data) => instance.post('/API/ControladorCentroCosto', data);
    update = (id, data) => instance.put(`/API/ControladorCentroCosto/${id}`, data);
    delete = (id) => instance.delete(`/API/ControladorCentroCosto/${id}`);

}

const CentroCosto = new BienCentroCostoService();
export default CentroCosto;