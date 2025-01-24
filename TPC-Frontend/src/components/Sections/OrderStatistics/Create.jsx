import React, { useState } from 'react'
import { Button, Modal, Form, Select } from 'antd'
import { alertSuccess } from '../../../utils/alert'
import OrdentEstadistica from '../../../service/OrdenEstadistica'
import useCentroCosto from '../../../hooks/useCentroCosto'
import { FloatInput, FloatSelect } from 'ant-float-label'

const Create = ({ refetch }) => {

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)


    const { data, isLoading, isSuccess, isError, error } = useCentroCosto()

    const onFinish = values => {

        setLoading(true)
        try {
            const id = data.find(item => item.nombre == values.Centro_de_Costo).id_Ceco

            OrdentEstadistica.create({
                ...values,
                Id_Centro_de_Costo: id.toString(),
                activado: true
            })
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Orden creada con éxito` })
                    refetch()
                })

        } catch (e) {
            console.log(e)
            setLoading(false)
            setModal(false)
        }
    }

    return (
        <div>
            <Button
                className="px-5"
                onClick={() => setModal(true)}
            >
                Agregar
            </Button>

            {modal && <Modal
                open={modal}
                title="Agregar Orden Estadistica"
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

                <Form name="create" onFinish={onFinish} preserve={false} className="pt-4 pb-2">

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
                            {data.map((item, index) => (
                                <Select.Option key={index} value={item.nombre}>{item.nombre}</Select.Option>
                            ))}
                        </FloatSelect>
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="Codigo_oe"
                        rules={[{
                            required: true,
                            message: 'Ingrese Orden de Compra'
                        }]}
                    >
                        <FloatInput
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
                        Crear
                    </Button>

                </Form>
            </Modal>}
        </div>
    )
}

export default Create