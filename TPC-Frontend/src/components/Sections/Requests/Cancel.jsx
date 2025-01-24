import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { LoadingOutlined } from '@ant-design/icons'
import { SlBan } from 'react-icons/sl';
import Request from '../../../service/Request';
import { alertSuccess } from '../../../utils/alert';


function Cancel({ solicitud }) {


    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)


    const onFinish = async () => {
        try {

            Request.delete(solicitud.iD_Cotizacion)
                .then(res => {
                    alertSuccess({ title: 'Solicitud cancelada', content: 'La solicitud ha sido cancelada correctamente' })
                })
                .catch(error => {
                    console.error(error)
                })

        } catch (error) {
            console.error(error)
        }
    }

    return (
        <div>

            <Button
                className="px-2"
                onClick={() => setModal(true)}
            >
                {loading ? <LoadingOutlined /> : <SlBan twoToneColor="#52c41a" />}
            </Button>
            {
                modal && <Modal
                    open={modal}
                    title="Cancelar Orden"
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
                    <div className="flex items-center justify-center gap-2">
                        <p>¿Estás seguro que deseas cancelar la solicitud <b>{solicitud.iD_Cotizacion}</b>?</p>
                        <Button
                            className="px-2"
                            onClick={() => setModal(false)}
                        >
                            Cancelar
                        </Button>
                        <Button
                            className="px-2"
                            onClick={() => onFinish()}
                        >
                            Aceptar
                        </Button>
                    </div>
                </Modal>
            }
        </div>
    )
}

export default Cancel