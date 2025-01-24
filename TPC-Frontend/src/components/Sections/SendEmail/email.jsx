import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { MdEmail } from 'react-icons/md'
import Correos from '../../../service/Correos'
import { alertSuccess } from '../../../utils/alert'

function Email({ selectedRowKeys, data, refetch }) {
    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)

    const onFinish = values => {
        try {

            Correos.contabilidad({
                Lista: selectedRowKeys.length > 0 ? selectedRowKeys : data.map(item => item.iD_Ticket),
            })
                .then(response => {
                    setLoading(false)
                    setModal(false)
                    alertSuccess({ message: `Correo enviado con éxito` })
                    refetch()
                    console.log(response)
                })

        } catch (e) {
            console.log(e)
            setLoading(false)
            setModal(false)
        }
    }

    return (
        <div>
            <Button
                className="px-5"
                onClick={() => setModal(true)}
            >
                <MdEmail size={20} />
                Enviar Correo
            </Button>
            {
                modal && <Modal
                    open={modal}
                    title="Enviar Correo"
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
                    <div className="flex flex-col items-center justify-center gap-2">
                        <p>{
                            selectedRowKeys.length > 0
                                ?
                                `
                                Se enviaran correos a el/los solicitantes seleccionados
                                `
                                : `No se ha seleccionado ninguna solicitud se enviara un correo a todos los solicitantes`

                        }</p>
                        <div className='flex  gap-5 items-center'>
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
                                Enviar
                            </Button>
                        </div>
                    </div>
                </Modal>
            }
        </div>
    )
}

export default Email