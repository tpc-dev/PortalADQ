import React, { useState } from 'react'
import { Button, Modal, Form, Input, Select } from 'antd'
import { alertError, alertSuccess } from '../../../utils/alert'
import User from '../../../service/User'
import { FloatInput, FloatSelect } from 'ant-float-label'
import useDepartament from '../../../hooks/useDepartament'
import useUsers from '../../../hooks/useUsers'
import Liberadores from '../../../service/Liberadores'

const Create = ({ refetch }) => {


    const { data, isLoading, isError } = useDepartament()

    const users = useUsers()

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const onFinish = values => {
        setLoading(true)
        try {

            Liberadores.post({
                ...values,
                id_Departamento: parseInt(values.id_Departamento),
                id_Usuario: parseInt(values.id_Usuario),
                Nombre_Usuario: users.data.filter(user => user.id_Usuario == values.id_Usuario)[0].nombre_Completo,
                Nombre_Departamento: data.filter(departament => departament.id_Departamento == values.id_Departamento)[0].nombre
            })
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Liberador  creado con éxito` })
                    refetch()
                })
                .catch((error) => {
                    alertError({ message: 'Error al crear Liberador' })
                    setLoading(false)
                    setModal(false)
                    console.log(error)
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
                title="Agregar Liberador"
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
                        name="id_Departamento"
                        rules={[{
                            required: true,
                            message: 'Ingrese Departamento'
                        }]}
                    >
                        <FloatSelect placeholder="Departamento" disabled={loading}>
                            {data && data.map((item, index) => (
                                <Select.Option key={index} value={item.id_Departamento}>{item.nombre}</Select.Option>
                            ))}
                        </FloatSelect>
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="id_Usuario"
                        rules={[{
                            required: true,
                            message: 'Ingrese Usuarios'
                        }]}
                    >
                        <FloatSelect placeholder="Usuario" disabled={loading}>
                            {data && users.data.filter(user => user.tipo_Liberador == true).map((item, index) => (
                                <Select.Option key={index} value={item.id_Usuario}>{item.nombre_Completo}</Select.Option>
                            ))}
                        </FloatSelect>
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
        </div >
    )
}

export default Create