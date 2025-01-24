import React, { useEffect, useState } from 'react'
import { Button, Modal, Form, Select } from 'antd'
import { alertSuccess } from '../../../utils/alert'
import Providers from '../../../service/Providers'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import useUsers from '../../../hooks/useUsers'
import Departamento from '../../../service/Departaments'
import { FloatInput, FloatSelect } from 'ant-float-label'

const Update = ({ departamento, refetch }) => {

    const { data, isLoading, isSuccess } = useUsers()

    const [form] = Form.useForm()

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const onFinish = () => {


        setLoading(true)
        try {

            const dataValues = {
                ...form.getFieldsValue(),
                Id_Departamento: departamento.id_Departamento,
                Encargado: typeof (form.getFieldValue('Encargado')) == 'string' ? departamento.id_Encargado.toString() : form.getFieldValue('Encargado').toString()
            }

            Departamento.update(departamento.id_Departamento, dataValues)
                .then((response) => {
                    console.log(response)
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Departamento creado con éxito` })

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
                Nombre: departamento.nombre,
                Descripcion: departamento.descripcion,
                Encargado: departamento.encargado
            })
        }
    }, [modal, departamento])

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
                title="Editar Departamento"
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
                        name="Descripcion"
                        rules={[{
                            required: true,
                            message: 'Ingrese Descripcion'
                        }]}
                    >
                        <FloatInput
                            placeholder="Descripcion"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="Encargado"
                        rules={[{
                            required: true,
                            message: 'Ingrese Encargado'
                        }]}
                    >
                        <FloatSelect placeholder="Encargado" disabled={loading}>
                            {data.filter(user => user.activado == true).map((user) => <Select.Option key={user.nombre_Completo} value={user.id_Usuario}>{user.nombre_Completo}</Select.Option>)}
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