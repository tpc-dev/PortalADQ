import React, { useEffect, useState } from 'react'
import { Button, Modal, Form, Select } from 'antd'
import { alertSuccess } from '../../../utils/alert'
import Providers from '../../../service/Providers'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import useBienServicio from '../../../hooks/useBienServicio'
import Request from '../../../service/Request'
import { FloatInput, FloatSelect } from 'ant-float-label'

const Update = ({ solicitud, refetch }) => {

    const { data, isLoading, isError, error, isSuccess } = useBienServicio()

    const [form] = Form.useForm()

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const [typeCotizacion, setTypeCotizacion] = useState(null)

    const onFinish = values => {
        setLoading(true)
        try {
            Request.update(solicitud.iD_Cotizacion, {
                ...values,
                ID_Cotizacion: solicitud.iD_Cotizacion,
                solped: values.solped ? Number(values.solped) : 0,
                detalle: values.detalle ? values.detalle : '',
                iD_Bien_Servicio: values.iD_Bien_Servicio ? typeof (values.iD_Bien_Servicio) == 'string' ?
                    data.find(item => item.bien_Servicio == values.bien_Servicio).iD_Bien_Servicio : values.iD_Bien_Servicio.toString() : 0

            })
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Solicitud Actualizada con éxito` })
                    refetch()
                })

        } catch (e) {
            setLoading(false)
            setModal(false)
        }
    }

    const changeForm = (e) => {


        if (e.target.value == '') {
            setTypeCotizacion(1)
            form.setFieldValue('bien_servicio', '')
            form.setFieldValue('detalle', '')
        } else {
            setTypeCotizacion(0)
        }

    }

    useEffect(() => {
        if (modal) {
            form.setFieldsValue(solicitud)
        }
    }, [modal, solicitud])

    useEffect(() => {
        if (solicitud.solped == 0) {
            setTypeCotizacion(1)
        } else {
            setTypeCotizacion(0)
        }
    }, [solicitud])



    return (
        <div>
            <Button
                className="px-2"
                onClick={() => setModal(true)}
            >
                {loading ? <LoadingOutlined /> : <EditOutlined twoToneColor="#52c41a" />}
            </Button>

            {modal && <Modal
                open={modal}
                title="Editar Solicitud"
                centered
                zIndex={3000}
                closable={true}
                destroyOnClose={true}
                maskClosable={false}
                keyboard={false}
                onCancel={() => setModal(false)}
                footer={null}
                width={600}
            >

                <Form form={form} name="edit" onFinish={onFinish} preserve={false} className="pt-4 pb-2">

                    <Form.Item
                        className="mb-4"
                        name="solped"
                        rules={[{
                            required: false,
                            message: 'Ingrese N° Solped'
                        }]}
                    >
                        <FloatInput
                            onChange={changeForm}
                            placeholder="N° Solped"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-4"
                        name="estado"
                        rules={[{
                            required: true,
                            message: 'Ingrese Estado'
                        }]}
                    >
                        <FloatSelect placeholder="Estado" disabled={loading}>
                            <Select.Option value="Recibida">Recibida</Select.Option>
                            <Select.Option value="Enviada a Proveedores">
                                Enviada a Proveedores
                            </Select.Option>
                            <Select.Option value='Cancelada'>Cancelada</Select.Option>
                        </FloatSelect>
                    </Form.Item>
                    {
                        typeCotizacion == 1 &&
                        <>
                            <Form.Item
                                className="mb-4"
                                name="detalle"
                                rules={[{
                                    required: true,
                                    message: 'Ingrese Detalle'
                                }]}
                            >
                                <FloatInput
                                    placeholder="Detalle"
                                    disabled={loading}
                                />
                            </Form.Item>



                            <Form.Item
                                className="mb-4"
                                name="iD_Bien_Servicio"
                                rules={[{
                                    required: true,
                                    message: 'Ingrese Bien/Servicio'
                                }]}
                            >
                                <FloatSelect placeholder="Bien/Servicio" disabled={loading}>

                                    {data.map((item, index) => {

                                        return (
                                            <Select.Option key={index} value={item.iD_Bien_Servicio}>{item.bien_Servicio}</Select.Option>
                                        )
                                    })}
                                </FloatSelect>
                            </Form.Item>
                        </>
                    }


                    <Button
                        type="primary"
                        htmlType="submit"
                        className="mt-4 px-5"
                        loading={loading}
                        disabled={loading}
                        block={true}
                    >
                        Editar
                    </Button>

                </Form>
            </Modal>}
        </div>
    )
}

export default Update