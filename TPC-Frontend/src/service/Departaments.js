import instance from "../apis/app";

class DepartamentoService {

    get = () => instance.get('/API/ControladorDepartamento');
    post = data => instance.post('/API/ControladorDepartamento', data)
    update = (id, data) => instance.put(`/API/ControladorDepartamento/${id}`, data)
    delete = id => instance.delete(`/API/ControladorDepartamento/${id}`)

}

const Departamento = new DepartamentoService();
export default Departamento