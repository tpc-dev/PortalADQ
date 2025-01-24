import { Button, Radio, Space } from 'antd'
import React from 'react'

function Solicitud({ Form, nextStep, tipoSolicitud, setTipoSolicitud }) {


    const handleSolicitud = ({ target }) => {
        const value = target.value
        setTipoSolicitud(value)
    }

    return (
        <div className='flex flex-col w-full'>
            <div className="flex flex-col gap-3 sm:gap-5 my-8">
                <div className="flex flex-col gap-3 sm:gap-4 items-start">
                    <label>Tipo de Solicitud</label>
                    <Form.Item name="solicitud" rules={[{ required: true, message: 'Por favor seleccione el tipo de solicitud' }]} className=' w-1/2 mb-0 flex flex-col gap-3'>
                        <Radio.Group onChange={handleSolicitud} >
                            <Space direction="vertical">
                                <Radio value={1}>Orden de compra</Radio>
                                <Radio value={2}>Cotizar</Radio>
                            </Space>
                        </Radio.Group>
                    </Form.Item>
                </div>
            </div>
            <Button className='self-end' onClick={nextStep} type="primary">Siguiente</Button>
        </div>
    )
}

export default Solicitud