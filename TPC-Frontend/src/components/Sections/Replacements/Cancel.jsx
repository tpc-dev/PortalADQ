import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { LoadingOutlined } from '@ant-design/icons'
import { SlBan } from 'react-icons/sl';
import Reemplazos from '../../../service/Replacements';
import { alertError, alertSuccess } from '../../../utils/alert';


function Cancel({ reemplazo, refetch }) {



    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)


    const onFinish = async () => {

        try {

            setLoading(true)
            Reemplazos.cancel(reemplazo.iD_Reemplazos)
                .then(response => {
                    setLoading(false)
                    alertSuccess({ message: `Reemplazo cancelado con éxito` })
                    setModal(false)
                    refetch()
                })
                .catch(error => {
                    console.log(error)
                    setLoading(false)
                    setModal(false)
                    alertError({ message: `Error al cancelar el reemplazo` })
                    refetch()
                })



        } catch (e) {
            console.log(e)
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
                    title="Cancelar Reemplazo"
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
                        <p>¿Estás seguro que deseas cancelar el reemplazo <b>{reemplazo.id}</b>?</p>
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