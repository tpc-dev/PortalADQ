import { Button, Form, Input, Modal, Select, Upload } from 'antd'
import React, { useEffect, useState } from 'react'
import { LoadingOutlined, UploadOutlined } from '@ant-design/icons'
import { MdEmail } from 'react-icons/md'
import useBienServicio from '../../../hooks/useBienServicio'
import Provider from '../../../service/Providers'
import Correos from '../../../service/Correos'
import { alertError, alertSuccess } from '../../../utils/alert'

function Cotizar({ cotizacion }) {

    console.log(cotizacion)

    const [form] = Form.useForm()
    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)
    const [bienServicioId, setBienServicioId] = useState(null)
    const [bienesYServicios, setBienesYServicios] = useState(null)
    const bienServicio = useBienServicio()
    const [file, setFile] = useState([])


    const onFinish = values => {
        setLoading(true)

        const formData = new FormData()
        formData.append('file', file[0])
        formData.append('Mensaje', values.Mensaje)
        formData.append('iD_Bien_Servicio', parseInt(values.bien_servicio))
        formData.append('Asunto', values.asunto)
        formData.append('Proveedor', [values.Proveedores])
        formData.append('Id_Cotizacion', cotizacion.iD_Cotizacion.toString())
        try {

            Correos.cotizar({ formData })
                .then(response => {
                    alertSuccess({ message: 'Cotizacion enviada con exito' })
                    setLoading(false)
                    setModal(false)
                })
                .catch(error => {
                    console.log(error)
                    alertError({ message: 'Error al enviar la cotizacion' })
                    setLoading(false)
                    setModal(false)
                })

        } catch (e) {
            console.log(e)
            setLoading(false)
            setModal(false)
        }
    }

    const propsUpload = {
        maxCount: 1,
        onRemove: file => setFile([]),
        beforeUpload: file => {
            setFile([file])
            return false
        },
        fileList: file
    }


    useEffect(() => {

        if (bienServicioId) {
            Provider.bienServicio(bienServicioId)
                .then(response => {
                    setBienesYServicios(response)
                })
                .catch(error => {
                    console.log(error)
                })
        }

    }, [bienServicioId])




    return (
        <div>
            {
                cotizacion.estado !== 'Cancelado' &&
                <Button
                    className="px-2"
                    onClick={() => setModal(true)}
                >
                    {loading ? <LoadingOutlined /> : <MdEmail twoToneColor="#52c41a" />}
                </Button>
            }
            {
                modal && <Modal
                    open={modal}
                    title="Enviar Cotizacion"
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
                    <Form name='cotizar' onFinish={onFinish} preserve={false} className="pt-4 pb-2">
                        <Form.Item
                            className="mb-2"
                            name="asunto"
                            rules={[{
                                required: true,
                                message: 'Ingrese el asunto'
                            }]}
                        >
                            <Input
                                placeholder="Asunto"
                                disabled={loading}
                            />
                        </Form.Item>
                        <Form.Item
                            className="mb-2"
                            name="bien_servicio"
                            rules={[{
                                required: true,
                                message: 'Ingrese el bien o servicio'
                            }]}
                        >
                            <Select
                                placeholder="Bien o Servicio"
                                disabled={loading}
                                onChange={(value) => setBienServicioId(value)}
                            >
                                {
                                    bienServicio.data?.map((item, index) => (
                                        <Select.Option key={index} value={item.iD_Bien_Servicio}>{item.bien_Servicio}</Select.Option>
                                    ))
                                }
                            </Select>
                        </Form.Item>

                        {
                            bienServicioId &&
                            <Form.Item
                                className="mb-2"
                                name="Proveedores"
                                rules={[{
                                    required: true,
                                    message: 'Ingrese la Proveedores'
                                }]}
                            >
                                <Select
                                    mode='multiple'
                                    placeholder="Proveedores"
                                    disabled={loading}

                                >
                                    {
                                        bienesYServicios?.map((item, index) => (
                                            <Select.Option key={index} value={item.iD_Proveedores}>{item.nombre_Fantasia}</Select.Option>
                                        ))
                                    }

                                </Select>

                            </Form.Item>
                        }

                        <Form.Item
                            className="mb-2"
                            name="Mensaje"
                            rules={[{
                                required: true,
                                message: 'Ingrese el mensaje'
                            }]}
                        >
                            <Input.TextArea
                                placeholder="Mensaje"
                                disabled={loading}
                            />
                        </Form.Item>

                        <Upload {...propsUpload} className="w-full">
                            <Button icon={<UploadOutlined />} block>Cargar archivo</Button>
                        </Upload>

                        <Button
                            type="primary"
                            htmlType="submit"
                            className="mt-4 px-5"
                            loading={loading}
                            disabled={loading}
                            block={true}
                        >
                            Enviar
                        </Button>

                    </Form>
                </Modal>

            }
        </div>
    )
}

export default Cotizar