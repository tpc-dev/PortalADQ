import instance from "../apis/app";

class RequestService {

    get = () => instance.get('/API/ControladorCotizacion');
    post = (data) => console.log(data) || instance.post('/API/ControladorCotizacion', data);
    update = (id, data) => instance.put(`/API/ControladorCotizacion/${id}`, data);
    delete = (id) => instance.delete(`/API/ControladorCotizacion/${id}`);
    Archivo = (id) => instance.get(`/API/ControladorCotizacion/Archivo/${id}`);
}

const Request = new RequestService();
export default Request;