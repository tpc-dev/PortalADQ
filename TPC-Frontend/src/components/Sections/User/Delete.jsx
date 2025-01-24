import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import { FaRegTrashAlt } from "react-icons/fa";
import OrdentEstadistica from '../../../service/OrdenEstadistica';
import { alertError, alertSuccess } from '../../../utils/alert';
import User from '../../../service/User';


function Delete({ user, refetch }) {

    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)


    const onFinish = async () => {
        try {

            User.delete(user.id_Usuario)
                .then(res => {
                    alertSuccess({ title: 'Usuario cancelado', content: 'El usuario ha sido eliminado correctamente' })
                    refetch()
                })
                .catch(error => {
                    console.error(error)
                    alertError({ title: 'Error', message: 'Ha ocurrido un error al eliminar el usuario' })
                })

        } catch (error) {
            alertError({ title: 'Error', message: 'Ha ocurrido un error al eliminar el usuario' })
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
                    title="Eliminar Usuario"
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
                        <p>¿Estás seguro que deseas eliminar el usuario <b>{user.id_Usuario}</b>?</p>
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