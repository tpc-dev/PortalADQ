import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { LoadingOutlined } from '@ant-design/icons'
import { SlBan } from 'react-icons/sl';
import { alertSuccess } from '../../../utils/alert';
import RequestOC from '../../../service/RequestOc';


function Cancel({ solicitud, refetch }) {

    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)



    const onFinish = async () => {
        try {

            RequestOC.delete(solicitud.iD_Ticket)
                .then(res => {
                    alertSuccess({ title: 'Solicitud cancelada', content: 'La solicitud ha sido cancelada correctamente' })
                    refetch()
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
                        <p>¿Estás seguro que deseas cancelar la solicitud <b>{solicitud.iD_Ticket}</b>?</p>
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