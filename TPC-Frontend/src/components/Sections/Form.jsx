import React, { useState } from 'react';
import { Form, Input, Button, Select, DatePicker, Steps, Breadcrumb } from 'antd';
import Modalidad from './modalidad';
import Solicitud from './solicitud';
import BackButton from '../Template/Backbutton';
import Request from '../../service/Request';
import useAuthContext from '../../hooks/useAuthContext';
import Confirmacion from './Confirmacion';
import RequestOC from '../../service/RequestOc';

function Create() {
    const { user } = useAuthContext();
    const [form] = Form.useForm();
    const [step, setStep] = useState(0);
    const [data, setData] = useState(null);
    const [modalidadState, setModalidadState] = useState(null);
    const [tipoSolicitud, setTipoSolicitud] = useState(null);
    const [loadingForm, setLoadingForm] = useState(false);
    const [succesfullData, setSuccesfullData] = useState(null);
    const [file, setFile] = useState(null); // Asegúrate de definir el estado para el archivo

    const nextStep = async () => {
        await form.validateFields()
            .then(response => setStep(step + 1))
            .catch(error => console.log(error));
    }

    const previousStep = () => setStep(step - 1);

    const onFinish = async (values) => {
        const formData = new FormData();
        formData.append('file', file); // Archivo
        formData.append('fileName', file.name); // Nombre
    
        try {
            const validatedValues = await form.validateFields();
    
            if (tipoSolicitud === 1) {
                // Orden de compra
                formData.append('solped', validatedValues.solped || 0);
                formData.append('Id_Proveedor', validatedValues.proveedor?.toString() || 0);
                formData.append('Id_OE', validatedValues.ordenEstadistica?.toString() || 0);
                formData.append('Estado', 'Recibido');
                formData.append('Id_Usuario', user.id_Usuario.toString());
                formData.append('Detalle', validatedValues.detalle || ' ');
    
                // Asegúrate de pasar formData correctamente
                RequestOC.post(formData)
                    .then(response => {
                        if (response.iD_Ticket) {
                            setStep(step + 1);
                            setSuccesfullData(response);
                            form.resetFields();
                        }
                    })
                    .catch(error => {
                        console.error('Error:', error);
                    });
            } else if (tipoSolicitud === 2) {
                // Cotización
                formData.append('Id_Solicitante', user.id_Usuario.toString());
                formData.append('Fecha_Creacion_Cotizacion', new Date().toISOString());
                formData.append('ID_Bien_Servicio', validatedValues.bienServicio ? validatedValues.bienServicio.toString() : '0');
                formData.append('Estado', 'Cotizacion Recibida');
                formData.append('Detalle', validatedValues.detalle ? validatedValues.detalle : '');
                formData.append('solped', validatedValues.solped ? validatedValues.solped : '0');
    
                // Asegúrate de pasar formData correctamente
                Request.post(formData)
                    .then(response => {
                        if (response.iD_Cotizacion) {
                            setStep(step + 1);
                            setSuccesfullData(response);
                            form.resetFields();
                        }
                    })
                    .catch(error => {
                        console.error('Error:', error);
                    });
            }
        } catch (error) {
            console.log('Validation Error:', error);
        }
    };
    

    const stepProps = { 
        Form, nextStep, previousStep, data, setData, loadingForm, 
        setModalidadState, modalidadState, tipoSolicitud, setTipoSolicitud, onFinish, setFile 
    };

    return (
        <div className="bg-white lg:shadow p-5 lg:p-5 lg:rounded flex flex-col items-center justify-center">
            <Breadcrumb
                items={[
                    { title: <a href="/solicitud">Inicio</a> },
                    { title: 'Crear solicitud' },
                    { title: 'Finalizar' }
                ]}
                className='text-sm mb-4'
            />

            <h1>Nueva solicitud</h1>

            <div className="w-full lg:w-2/4 mt-8">
                <Form name='solicitud' form={form} className='flex flex-col gap-5' disabled={loadingForm}>
                    <Steps current={step} responsive={false} items={[
                        { title: 'Modalidad' },
                        { title: 'Tipo de solicitud' },
                    ]} />
                    {step === 0 && <Solicitud {...stepProps} />}
                    {step === 1 && <Modalidad {...stepProps} />}
                    {step === 2 && <Confirmacion data={succesfullData} />}
                </Form>
            </div>
        </div>
    );
}

export default Create;
