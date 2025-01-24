import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import { FaRegTrashAlt } from "react-icons/fa";
import OrdentEstadistica from '../../../service/OrdenEstadistica';
import { alertError, alertSuccess } from '../../../utils/alert';


function Delete({ order, refetch }) {

    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)



    const onFinish = async () => {
        try {

            OrdentEstadistica.delete(order.id_Orden_Estadistica, {
                id_Orden_Estadistica: order.id_Orden_Estadistica,
            })
                .then(res => {
                    alertSuccess({ title: 'Solicitud cancelada', content: 'La orden estadistica ha sido cancelada correctamente' })
                    refetch()
                })
                .catch(error => {
                    console.error(error)
                    alertError({
                        title: 'Error', content: 'Ha ocurrido un error al cancelar la solicitud'})
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
                            <p>¿Estás seguro que deseas eliminar la orden <b>{order.id_Orden_Estadistica}</b>?</p>
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