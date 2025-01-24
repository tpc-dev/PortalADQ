import React, { useEffect, useState } from 'react'
import { Button, Modal, Form, Input, Select } from 'antd'
import { alertSuccess } from '../../../utils/alert'
import { EditOutlined, LoadingOutlined, LockOutlined } from '@ant-design/icons'
import User from '../../../service/User'
import { FloatInput, FloatSelect } from 'ant-float-label'

const Update = ({ user, refetch }) => {



    const [form] = Form.useForm()

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const onFinish = values => {

        setLoading(true)
        try {
            User.update(user.id_Usuario, {
                ...values,
                id_Usuario: user.id_Usuario,
                en_Vacaciones: values.en_Vacaciones == '0' ? false : true,
                admin: values.rol == '0' ? false : true,
                Activado: true,
                tipo_Liberador: values.tipo_Liberador == 'No' ? false : true,
                ListaIdDep: [0],
                ListaDepartamento: ['']
            })
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Usuario Actualizada con éxito` })
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
                ...user,
                nombre_usuario: user.nombre_Usuario,
                Rut_Usuario: user.rut_Usuario,
                Correo_Usuario: user.correo_Usuario,
                en_Vacaciones: user.en_Vacaciones == false ? '0' : '1',
                rol: user.admin == false ? '0' : '1',
                tipo_Liberador: user.tipo_Liberador == false ? 'No' : 'Si',
                contraseña_Usuario: ''
            })
        }
    }, [modal, user])

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
                title="Editar Usuario"
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
                        name="contraseña_Usuario"
                        rules={[{
                            required: true,
                            message: 'Ingrese Contraseña'
                        }]}

                    >
                        <FloatInput placeholder="Contraseña" disabled={loading} >
                        </FloatInput>
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
                        Guardar
                    </Button>

                </Form>
            </Modal>}
        </div>
    )
}

export default Update