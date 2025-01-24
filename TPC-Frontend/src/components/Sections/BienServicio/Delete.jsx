import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import { FaRegTrashAlt } from "react-icons/fa";
import OrdentEstadistica from '../../../service/OrdenEstadistica';
import { alertError, alertSuccess } from '../../../utils/alert';
import BienServicio from '../../../service/Bien_Servicios';


function Delete({ bienServicio, refetch }) {

    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)


    console.log(bienServicio)


    const onFinish = async () => {
        try {

            BienServicio.delete(bienServicio.iD_Bien_Servicio)
                .then(res => {
                    alertSuccess({ title: 'Bien y servicio cancelado', message: 'El bien y servicio ha sido cancelado correctamente' })
                    refetch()
                })
                .catch(error => {
                    alertError({ title: 'Error', message: 'Ha ocurrido un error al cancelar el bien y servicio' })
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
                        <p>¿Estás seguro que deseas eliminar el bien y servicio <b>{bienServicio.iD_Bien_Servicio}</b>?</p>
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