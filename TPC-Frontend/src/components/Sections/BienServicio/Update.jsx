import React, { useEffect, useState } from 'react'
import { Button, Modal, Form, Input, Select } from 'antd'
import { alertSuccess } from '../../../utils/alert'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import BienServicio from '../../../service/Bien_Servicios'
import { FloatInput } from 'ant-float-label'

const Update = ({ bienServicio, refetch }) => {

    const [form] = Form.useForm()

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const onFinish = values => {
        setLoading(true)
        try {
            BienServicio.update(bienServicio.iD_Bien_Servicio, {
                bien_Servicio: values.bien_Servicio,
                iD_Bien_Servicio: bienServicio.iD_Bien_Servicio
            })
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Bien y Servicio Actualizada con éxito` })
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
            form.setFieldsValue(bienServicio)
        }
    }, [bienServicio, modal])

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
                title="Editar Bien y Servicio"
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
                        name="bien_Servicio"
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