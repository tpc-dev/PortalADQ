import instance from "../apis/app";

class ExcelService {

    OC = () => instance.get('/API/ControladorOrdenCompra/Imprimir', { responseType: 'blob' });
    Requests = () => instance.get('/API/ControladorCotizacion/Imprimir', { responseType: 'blob' });
    Users = () => instance.get('/API/ControladorUsuario/Imprimir', { responseType: 'blob' });
    OCExcel = () => instance.post('/API/ControladorExcel/OCA');
    SingleProvider = (data) => instance.post('/API/ControladorExcel/Proveedor', data.formData, { headers: { 'Content-Type': 'multipart/form-data' } });
    CostCenter = (data) => console.log(data) || instance.post('/API/ControladorExcel/CeCo', data.formData, { headers: { 'Content-Type': 'multipart/form-data' } });
    OCA = (data) => instance.post('/API/ControladorExcel/OCA', data.formData, { headers: { 'Content-Type': 'multipart/form-data' } });
    BienServicio = (data) => instance.post('/API/ControladorExcel/BS', data.formData, { headers: { 'Content-Type': 'multipart/form-data' } });
    SubirOC = (data) => instance.post('/API/ControladorExcel/poss', data.formData, { headers: { 'Content-Type': 'multipart/form-data' } });


}

const Excels = new ExcelService();
export default Excels;