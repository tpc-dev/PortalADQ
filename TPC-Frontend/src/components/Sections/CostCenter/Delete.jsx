import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import { FaRegTrashAlt } from "react-icons/fa";
import { alertError, alertSuccess } from '../../../utils/alert';
import CentroCosto from '../../../service/CentroCosto';


function Delete({ Ceco, refetch }) {

    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)


    const onFinish = async () => {
        try {

            CentroCosto.delete(Ceco.id_Ceco)
                .then(res => {
                    alertSuccess({ title: 'Solicitud cancelada', content: 'La solicitud ha sido cancelada correctamente' })
                    refetch()
                })
                .catch(error => {
                    alertError({ title: 'Error', content: 'Ha ocurrido un error al cancelar la solicitud' })
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
                    title="Eliminar Centro"
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
                        <p>¿Estás seguro que deseas eliminar el <b>{Ceco.id_Ceco}</b>?</p>
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