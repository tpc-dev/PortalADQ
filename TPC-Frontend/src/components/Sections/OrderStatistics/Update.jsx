import React, { useEffect, useState } from 'react'
import { Button, Modal, Form, Input, Select } from 'antd'
import { alertSuccess } from '../../../utils/alert'
import Providers from '../../../service/Providers'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import useCentroCosto from '../../../hooks/useCentroCosto'
import OrdentEstadistica from '../../../service/OrdenEstadistica'
import { FloatInput, FloatSelect } from 'ant-float-label'

const Update = ({ order, refetch }) => {

    const { data, isLoading, isSuccess, isError, error } = useCentroCosto()

    const [form] = Form.useForm()

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const onFinish = values => {
        setLoading(true)
        try {
            const id = data.find(item => item.nombre == form.getFieldValue('Centro_de_Costo')).id_Ceco
            OrdentEstadistica.update(order.id_Orden_Estadistica, {
                Nombre: form.getFieldValue('Nombre'),
                Id_Centro_de_Costo: id.toString(),
                codigo_OE: form.getFieldValue('codigo_OE'),
                Id_Orden_Estadistica: order.id_Orden_Estadistica
            })
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Orden Actualizada con éxito` })
                    refetch()

                })

        } catch (e) {
            console.log(e)
            setLoading(false)
            setModal(false)
        }
    }

    useEffect(() => {
        if (modal) {
            form.setFieldsValue({
                Nombre: order.nombre,
                codigo_OE: order.codigo_OE,
                Centro_de_Costo: order.id_Centro_de_Costo
            })
        }
    }, [modal, order])

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
                title="Editar Orden Estadistica"
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
                        name="Nombre"
                        rules={[{
                            required: true,
                            message: 'Ingrese Nombre'
                        }]}
                    >
                        <FloatInput
                            placeholder="Nombre"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="Centro_de_Costo"
                        rules={[{
                            required: true,
                            message: 'Ingrese Centro de Costo'
                        }]}
                    >
                        <FloatSelect placeholder="Centro de Costo" disabled={loading} showSearch>
                            {isSuccess && data.map((item, index) => (
                                <Select.Option key={index} value={item.nombre}>{item.nombre}</Select.Option>
                            ))}
                        </FloatSelect>
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="codigo_OE"
                        rules={[{
                            required: true,
                            message: 'Ingrese Orden de Compra'
                        }]}
                    >
                        <Input
                            placeholder="Orden de Compra"
                            disabled={loading}
                        />
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
        </div>
    )
}

export default Update