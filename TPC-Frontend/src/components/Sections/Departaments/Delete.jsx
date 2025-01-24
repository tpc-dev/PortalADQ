import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { EditOutlined, LoadingOutlined } from '@ant-design/icons'
import { FaRegTrashAlt } from "react-icons/fa";
import Departamento from '../../../service/Departaments';
import { alertSuccess } from '../../../utils/alert';


function Delete({ departamento, refetch }) {

    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)

    const onFinish = async () => {
        try {
            setLoading(true)
            setModal(false)
            Modal.warning({
                title: 'Advertencia',
                content: 'Esta seguro de Eliminar?',
                onOk: () => {
                    Departamento.delete(departamento.id_Departamento)
                        .then(res => {
                            alertSuccess({ title: 'Departamento cancelada', content: 'El departamento ha sido cancelado' })
                            refetch()
                            setLoading(false)

                        })
                        .catch(error => {
                            console.error(error)
                        })
                }
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
                    title="Eliminar Departamento"
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
                        <p>¿Estás seguro que deseas eliminar el departamento <b>{departamento.id_Departamento}</b>?</p>
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