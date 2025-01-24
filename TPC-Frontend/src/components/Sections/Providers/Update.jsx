import React, { useEffect, useState } from 'react'
import { Button, Modal, Form, Select, Checkbox } from 'antd'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import { alertSuccess, alertError } from '../../../utils/alert'
import Provider from '../../../service/Providers'
import useBienServicio from '../../../hooks/useBienServicio'
import { FloatInput, FloatSelect } from 'ant-float-label'

const Update = ({ proveedor, refetch }) => {

    const { data, isLoading, isSuccess } = useBienServicio()

    const [form] = Form.useForm()

    const [loading, setLoading] = useState(false)

    const [modal, setModal] = useState(false)

    const onFinish = () => {

        setLoading(true)
        try {
            const id = data.find(item => item.bien_Servicio == form.getFieldValue('iD_Bien_Servicio')).iD_Bien_Servicio
            const dataValues = {
                ...form.getFieldsValue(),
                ID_Proveedores: proveedor.iD_Proveedores.toString(),
                iD_Bien_Servicio: id.toString(),
                Estado: form.getFieldValue('Estado') === '1' ? true : false
            }
            Provider.update(dataValues, proveedor.iD_Proveedores)
                .then((response) => {
                    setModal(false)
                    setLoading(false)
                    alertSuccess({ message: `Proveedor actualizado con éxito` })
                    refetch()
                })

        } catch (error) {
            console.log(error)
            alertError({ message: error.message })
        }
        setModal(false)
        setLoading(false)
        alertSuccess({ message: `Usuario actualizado con éxito` })
    }

    useEffect(() => {
        if (modal) {
            form.setFieldsValue(proveedor)
        }
    }, [modal, proveedor])



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
                title="Editar Proveedor"
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
                        name="rut_Proveedor"
                        rules={[{
                            required: true,
                            message: 'Ingrese Rut de Proveedor'
                        }]}
                    >
                        <FloatInput
                            placeholder="Rut"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="razon_Social"
                        rules={[{
                            required: true,
                            message: 'Ingrese Razón Social'
                        }]}
                    >
                        <FloatInput
                            placeholder="Razón Social"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="nombre_Fantasia"
                        rules={[{
                            required: true,
                            message: 'Ingrese Nombre Fantasía'
                        }]}
                    >
                        <FloatInput
                            placeholder="Nombre Fantasía"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="iD_Bien_Servicio"
                        rules={[{
                            required: true,
                            message: 'Ingrese Bien/Servicio'
                        }]}
                    >
                        <FloatSelect
                            placeholder="Bien/Servicio"
                            disabled={loading}
                        >
                            {data.map((item, index) => (
                                <Select.Option key={index} value={item.bien_Servicio}>{item.descripcion}</Select.Option>
                            ))}
                        </FloatSelect>

                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="direccion"
                        rules={[{
                            required: true,
                            message: 'Ingrese Dirección'
                        }]}>
                        <FloatInput
                            placeholder="Dirección"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="comuna"
                        rules={[{
                            required: true,
                            message: 'Ingrese Comuna'
                        }]}
                    >
                        <FloatInput
                            placeholder="Comuna"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="correo_Proveedor"
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
                        name="telefono_Proveedor"
                        rules={[{
                            required: true,
                            message: 'Ingrese Contacto'
                        }]}
                    >
                        <FloatInput
                            placeholder="Contacto"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="cargo_Representante"
                        rules={[{
                            required: true,
                            message: 'Ingrese Cargo Representante'
                        }]}
                    >
                        <FloatInput
                            placeholder="Cargo Representante"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="nombre_Representante"
                        rules={[{
                            required: true,
                            message: 'Ingrese Nombre Representante'
                        }]}
                    >
                        <FloatInput
                            placeholder="Nombre Representante"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="email_Representante"
                        rules={[{
                            required: true,
                            message: 'Ingrese Correo Representante'
                        }]}
                    >
                        <FloatInput
                            placeholder="Correo Representante"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="Estado"
                        rules={[{
                            required: false,
                            message: 'Ingrese Estado'
                        }]}>
                        <Select placeholder="Estado" disabled={loading}
                            defaultValue={proveedor.Estado == true ? '1' : '0'}
                        >

                            <Select.Option value="0">Inactivo</Select.Option>                  
                            <Select.Option value="1">Activo</Select.Option>
                        </Select>
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="n_Cuenta"
                        rules={[{

                            message: 'Ingrese Número de Cuenta'
                        }]}
                    >
                        <FloatInput
                            placeholder="Número de Cuenta"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="banco"
                        rules={[{
                            required: false,
                        }]}
                    >
                        <FloatInput
                            placeholder="Banco"
                            disabled={loading}
                        />
                    </Form.Item>

                    <Form.Item
                        className="mb-2"
                        name="swift1"
                        rules={[{
                            required: false,
                        }]}
                    >
                        <FloatInput
                            placeholder="Swift"
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
                        Guardar
                    </Button>

                </Form>
            </Modal>}
        </div>
    )
}

export default Update