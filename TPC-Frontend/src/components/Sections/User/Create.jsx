import React, { useState } from 'react'
import { Button, Modal, Form, Input, Select } from 'antd'
import { alertError, alertSuccess } from '../../../utils/alert'
import User from '../../../service/User'
import { FloatInput, FloatSelect } from 'ant-float-label'
import useDepartament from '../../../hooks/useDepartament'

const Create = ({ refetch }) => {


    const { data, isLoading, isError } = useDepartament()

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const onFinish = values => {
        setLoading(true)
        try {

            User.post({
                ...values,
                tipo_Liberador: values.tipo_Liberador === 'No' ? false : true,
                en_Vacaciones: values.en_Vacaciones === '1' ? true : false,
                admin: values.rol === '1' ? true : false,
                Id_Departamento: parseInt(values.id_Departamento),
                ListaIdDep: [0],
                ListaDepartamento: ['']

            })
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Usuario creado con éxito` })
                    refetch()
                })
                .catch((error) => {
                    alertError({ message: 'Error al crear usuario' })
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

    console.log(data)

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
                title="Agregar Usuario"
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
                        name="nombre_usuario"
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
                        name="apellido_paterno"
                        rules={[{
                            required: true,
                            message: 'Ingrese Apellido Paterno'
                        }]}
                    >
                        <FloatInput
                            placeholder="Apellido Paterno"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="apellido_materno"
                        rules={[{
                            required: true,
                            message: 'Ingrese Apellido Materno'
                        }]}
                    >
                        <FloatInput
                            placeholder="Apellido Materno"
                            disabled={loading}
                        />
                    </Form.Item>
                    <Form.Item
                        className="mb-2"
                        name="Rut_Usuario"
                        rules={[{
                            required: true,
                            message: 'Ingrese Rut'
                        }]}
                    >
                        <FloatInput
                            placeholder="Rut"
                            disabled={loading}
                        />
                    </Form.Item>
                    <Form.Item
                        className="mb-2"
                        name="tipo_Liberador"
                        rules={[{
                            required: true,
                            message: 'Ingrese Liberador'
                        }]}
                    >
                        <FloatSelect placeholder="Liberador" disabled={loading}>
                            <Select.Option value="No">No</Select.Option>
                            <Select.Option value="Si">Si</Select.Option>
                        </FloatSelect>
                    </Form.Item>

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
                        name="Correo_Usuario"
                        rules={[{
                            required: true,
                            message: 'Ingrese Correo'
                        }]}
                    >
                        <FloatInput
                            placeholder="Correo"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="en_Vacaciones"
                        rules={[{
                            required: true,
                            message: 'Ingrese si esta en vacaciones'
                        }]}
                    >
                        <FloatSelect placeholder="Vacaciones" disabled={loading}>
                            <Select.Option value="1">Si</Select.Option>
                            <Select.Option value="0">No</Select.Option>
                        </FloatSelect>
                    </Form.Item>


                    <Form.Item
                        className="mb-2"
                        name="rol"
                        rules={[{
                            required: true,
                            message: 'Ingrese Rol'
                        }]}
                    >
                        <FloatSelect placeholder="Rol" disabled={loading}>
                            <Select.Option value="1">Administrador</Select.Option>
                            <Select.Option value="0">Usuario</Select.Option>
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