import React, { useEffect, useState } from 'react'
import { Button, Modal, Form, Input, Select } from 'antd'
import { alertSuccess } from '../../../utils/alert'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import Liberadores from '../../../service/Liberadores'
import { FloatInput, FloatSelect } from 'ant-float-label'
import useDepartament from '../../../hooks/useDepartament'
import useUsers from '../../../hooks/useUsers'

const Update = ({ liberador, refetch }) => {


    const [form] = Form.useForm()

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const { data, isLoading, isError } = useDepartament()

    const users = useUsers()

    const onFinish = values => {

        setLoading(true)
        try {

            console.log(values)

            const User = typeof (values.id_Usuario) == "string" ? users.data.find(user => user.nombre_Usuario == values.id_Usuario).id_Usuario : values.id_Usuario

            Liberadores.update(liberador.id_Liberador, {
                id_Liberador: liberador.id_Liberador,
                id_Usuario: User,
                nombre_Usuario: typeof (values.id_Usuario) == "string" ? users.data.find(user => user.nombre_Usuario == values.id_Usuario).nombre_Usuario : liberador.nombre_Usuario,
                id_Departamento: liberador.id_Departamento,
                nombre_Departamento: liberador.nombre_Departamento

            })
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Liberador Actualizada con éxito` })
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
                id_Departamento: liberador.nombre_Departamento,
                id_Usuario: liberador.nombre_Usuario
            })
        }
    }, [modal, liberador])

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
                title="Editar Liberador"
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
                        Guardar
                    </Button>

                </Form>
            </Modal>}
        </div>
    )
}

export default Update