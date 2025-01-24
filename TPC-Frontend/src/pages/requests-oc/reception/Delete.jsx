import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import { FaRegTrashAlt } from "react-icons/fa";
import { alertSuccess } from '../../../utils/alert';
import RequestOC from '../../../service/RequestOc';


function Delete({ OC }) {

    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)

    const onFinish = async () => {
        try {

            RequestOC.deleteOC(OC.id_Orden_Compra)
                .then(res => {
                    alertSuccess({ title: 'Orden cancelada', content: 'La orden  ha sido cancelada correctamente' })
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
                {loading ? <LoadingOutlined /> : <FaRegTrashAlt twoToneColor="#52c41a" />}
            </Button>
            {
                modal && <Modal
                    open={modal}
                    title="Eliminar Orden"
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
                        <p>¿Estás seguro que deseas eliminar la orden <b>{OC.id_Orden_Compra}</b>?</p>
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
                            Eliminar
                        </Button>
                    </div>
                </Modal>
            }
        </div>
    )
}

export default Delete