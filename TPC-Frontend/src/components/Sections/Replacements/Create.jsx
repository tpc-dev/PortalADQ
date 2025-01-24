import React, { useState } from 'react'
import { Button, Modal, Form, Input, Select, DatePicker } from 'antd'
import { alertSuccess } from '../../../utils/alert'
import Providers from '../../../service/Providers'
import useUsers from '../../../hooks/useUsers'
import Reemplazos from '../../../service/Replacements'
import { FloatInput, FloatSelect } from 'ant-float-label'

const Create = ({ refetch }) => {

    const { data, isLoading, isSuccess } = useUsers()
    const [loading, setLoading] = useState(false)
    const [date, setDate] = useState('')
    const [form] = Form.useForm()
    const [userVacaciones, setUserVacaciones] = useState(null)

    const [modal, setModal] = useState(false)
    const onFinish = values => {

        setLoading(true)
        try {
            console.log(values)
            Reemplazos.create({
                id_Usuario_Vacaciones: values.id_Usuario_Vacaciones.toString(),
                id_Usuario_Reemplazante: values.id_Usuario_Reemplazante.toString(),
                comentario: values.comentario,
                fecha_Retorno: date
            })
                .then((response) => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Reemplazo creado con éxito` })
                    setUserVacaciones(null)
                    refetch()
                })

        } catch (e) {
            console.log(e)
            setLoading(false)
            setModal(false)
        }
    }

    const onChange = (date, dateString) => {
        var dateParse = new Date(dateString);
        setDate(dateParse)
    }

    return (
        <div>
            <Button
                className="px-5"
                onClick={() => setModal(true)}
            >
                Agregar
            </Button>

            {modal && isSuccess && <Modal
                open={modal}
                title="Agregar Reemplazo"
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

                <Form form={form} name="create" onFinish={onFinish} preserve={false} className="pt-4 pb-2">

                    <Form.Item
                        className="mb-2"
                        name="id_Usuario_Vacaciones"
                        rules={[{
                            required: true,
                            message: 'Ingrese Nombre de la persona que se va de vacaciones'
                        }]}
                    >
                        <FloatSelect disabled={loading} placeholder="Persona que se va de vacaciones" onChange={(e) =>
                            setUserVacaciones(e)
                        }>
                            {data.filter((user) => user.en_Vacaciones == true).map((user, index) => (
                                <Select.Option key={index} value={user.id_Usuario}>{
                                    `${user.nombre_Usuario} ${user.apellido_paterno} ${user.apellido_materno}`
                                }</Select.Option>
                            ))}
                        </FloatSelect>
                    </Form.Item>

                    {
                        userVacaciones !== null &&
                        <Form.Item
                            className="mb-2"
                            name="id_Usuario_Reemplazante"
                            rules={[{
                                required: true,
                                message: 'Ingrese Nombre del reemplazante'
                            }]}
                        >
                            <FloatSelect disabled={loading} placeholder="Reemplazante">
                                {
                                    userVacaciones !== null ?
                                        data.filter((user) => user.en_Vacaciones == false).map((user, index) => {
                                            if (user.id_Usuario !== userVacaciones) {
                                                return <Select.Option key={index} value={user.id_Usuario}>{
                                                    `${user.nombre_Usuario} ${user.apellido_paterno} ${user.apellido_materno}`
                                                }</Select.Option>
                                            }
                                        }) : null
                                }
                            </FloatSelect>
                        </Form.Item>
                    }


                    <Form.Item
                        className="mb-2"
                        name="comentario"
                        rules={[{
                            required: true,
                            message: 'Ingrese un comentario'
                        }]}
                    >
                        <FloatInput
                            placeholder="Comentario"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="fecha_Retorno"
                        rules={[{
                            required: true,
                            message: 'Ingrese la fecha de retorno'
                        }]}
                    >
                        <DatePicker onChange={onChange} disabled={loading} />
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