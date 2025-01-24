import instance from "../apis/app";


class UserService {

    post = data => instance.post('/API/ControladorUsuario', data);
    update = (id, data) => instance.put(`/API/ControladorUsuario/${id}`, data);
    get = id => instance.get(`/API/ControladorUsuario/${id}`);
    list = () => instance.get('/API/ControladorUsuario');
    auth = data => instance.post('/API/ControladorAutentizar', data);
    OTP = data => instance.post('/API/ControladorAutentizar/MFA', data);
    delete = id => instance.delete(`/API/ControladorUsuario/${id}`);
    newUser = data => instance.post('/API/ControladorAutentizar/nuevo', data);
    recover = data => instance.post('/API/ControladorAutentizar/pass', data);
}

const User = new UserService();
export default User;