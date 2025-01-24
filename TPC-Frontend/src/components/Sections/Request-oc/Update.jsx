import React, { useEffect, useState } from 'react'
import { Button, Modal, Form, Input, Select } from 'antd'
import { alertSuccess } from '../../../utils/alert'
import Providers from '../../../service/Providers'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import Status from '../../../data/Status.json'
import useProviders from '../../../hooks/useProviders'
import useOrdenesEstadisticas from '../../../hooks/useOrdenesEstadisticas'
import RequestOC from '../../../service/RequestOc'
import { FloatInput, FloatSelect } from 'ant-float-label'

const Update = ({ solicitud, refetch }) => {

    const { data, isLoading, isError, error, isSuccess } = useProviders()


    const ordenEstadistica = useOrdenesEstadisticas()

    const [form] = Form.useForm()

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const [type, setType] = useState(0)

    const onFinish = values => {

        setLoading(true)
        try {
            RequestOC.update(solicitud.iD_Ticket, {
                estado: values.estado,
                detalle: values.detalle ? values.detalle : '',
                solped: values.solped ? values.solped : 0,
                n_OE: values.id_OE ? typeof (values.id_OE) == 'string' ? ordenEstadistica.data.find(item => item.nombre == values.id_OE).id_Orden_Estadistica.toString() : values.id_OE.toString() : null,
                iD_Proveedor: values.iD_Proveedor ? typeof (values.iD_Proveedor) == 'string' ? data.find(item => item.nombre_Fantasia == values.iD_Proveedor).iD_Proveedores.toString() : values.iD_Proveedor.toString() : null,
                Numero_OC: values.numero_OC ? Number(values.numero_OC) : 0,
                iD_Ticket: solicitud.iD_Ticket,
                fecha_Creacion_OC: solicitud.fecha_Creacion_OC ? solicitud.fecha_Creacion_OC : new Date(),
                fecha_OC_Enviada: solicitud.fecha_OC_Enviada ? solicitud.fecha_OC_Enviada : new Date(),
                fecha_OC_Liberada: solicitud.fecha_OC_Liberada ? solicitud.fecha_OC_Liberada : new Date(),
                fecha_OC_Recepcionada: solicitud.fecha_OC_Recepcionada ? solicitud.fecha_OC_Recepcionada : new Date(),

            })
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Solicitud Actualizada con éxito` })
                    refetch()

                })

        } catch (e) {
            console.log(e)
            setLoading(false)
            setModal(false)
        }


    }

    const changeForm = (e) => {
        if (e.target.value == '' || e.target.value == 0) {
            setType(1)
            form.setFieldValue('detalle', '')
        } else {
            setType(0)
        }

    }

    useEffect(() => {
        if (modal) {
            form.setFieldsValue(solicitud)
        }
    }, [modal, solicitud])




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
                        className="mb-2"
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
                        className="mb-2"
                        name="estado"
                        rules={[{
                            required: true,
                            message: 'Ingrese Estado'
                        }]}
                    >
                        <FloatSelect placeholder="Estado" disabled={loading}>
                            {
                                Status.map((element, index) => (
                                    <Select.Option key={index} value={element.name}>{element.name}</Select.Option>
                                ))
                            }
                        </FloatSelect>
                    </Form.Item>

                    {
                        type !== 0 &&
                        <Form.Item
                            className="mb-2"
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
                    }

                    <Form.Item
                        className="mb-2"
                        name="iD_Proveedor"
                        rules={[{
                            required: true,
                            message: 'Ingrese Proveedor'
                        }]}
                    >
                        <FloatSelect showSearch placeholder="Proveedor" disabled={loading}>
                            {
                                data.map((element, index) => (
                                    <Select.Option key={index} value={element.iD_Proveedores}>{element.nombre_Fantasia}</Select.Option>
                                ))
                            }
                        </FloatSelect>
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="id_OE"
                        rules={[{
                            required: true,
                            message: 'Ingrese Orden Estadistica'
                        }]}
                    >
                        <FloatSelect placeholder="Orden Estadistica" disabled={loading}>
                            {
                                ordenEstadistica.data.map((element, index) => (
                                    <Select.Option key={index} value={element.id_Orden_Estadistica}>{element.nombre}</Select.Option>
                                ))
                            }
                        </FloatSelect>

                    </Form.Item>
                    <Form.Item
                        className="mb-2"
                        name="numero_OC"
                        rules={[{
                            required: true,
                            message: 'Ingrese Numero OC'
                        }]}
                    >
                        <FloatInput placeholder="Numero OC" disabled={loading} />

                    </Form.Item>


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
        </div >
    )
}

export default Update