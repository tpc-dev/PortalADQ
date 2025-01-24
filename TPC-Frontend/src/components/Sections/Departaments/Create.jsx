import React, { useState } from 'react'
import { Button, Modal, Form, Select } from 'antd'
import { alertSuccess } from '../../../utils/alert'
import Departamento from '../../../service/Departaments'
import useUsers from '../../../hooks/useUsers'
import { FloatInput, FloatSelect } from 'ant-float-label'

const Create = ({ refetch }) => {

    const { data, isLoading, isSuccess } = useUsers()



    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const onFinish = values => {
        setLoading(true)
        try {

            const id = data.find(u => u.nombre_Completo == values.Encargado).id_Usuario

            const dataValues = {
                ...values,
                Encargado: id.toString(),
            }

            Departamento.post(dataValues)
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Departamento creado con éxito` })
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
                title="Agregar Departamento"
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

                    {
                        isSuccess &&
                        <Form.Item
                            className="mb-2"
                            name="Encargado"
                            rules={[{
                                required: true,
                                message: 'Ingrese Encargado'
                            }]}
                        >
                            <FloatSelect placeholder="Encargado" disabled={loading} showSearch>
                                {data?.filter(u => u.activado == true).map(u => <Select.Option key={u.id_Usuario} value={u.nombre_Completo}>
                                    {
                                        u.nombre_Completo
                                    }
                                </Select.Option>)}
                            </FloatSelect>
                        </Form.Item>
                    }

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