import { Button, Modal, Steps } from 'antd'
import React, { useEffect, useState } from 'react'
import { FaShoppingCart } from 'react-icons/fa'
import { LoadingOutlined } from '@ant-design/icons'

function Cart({ record }) {
    const [loading, setLoading] = useState(false)
    const [modal, setModal] = useState(false)
    const [step, setStep] = useState(0)


    console.log(record)

    const changeStep = () => {

        if (record.estado == 'OC Enviada') {
            setStep(3)
        }

        if (record.estado == 'OC Parcial') {
            setStep(4)
        }

        if (record.estado == 'OC Recepcionada') {
            setStep(5)
        }



    }

    useEffect(() => {
        changeStep()
    }, [record])


    return (
        <div>
            <Button
                className="px-2"
                onClick={() => setModal(true)}
            >
                {loading ? <LoadingOutlined /> : <FaShoppingCart twoToneColor="#52c41a" />}
            </Button>
            {
                modal && <Modal
                    open={modal}
                    title="Estado de la Orden"
                    centered
                    zIndex={3000}
                    closable={true}
                    destroyOnClose={true}
                    maskClosable={false}
                    keyboard={false}
                    onCancel={() => setModal(false)}
                    footer={null}
                    width={1000}
                >
                    <div className="flex items-center justify-center gap-2 p-5">

                        {
                            record.estado == 'OC Cancelada' || record.estado == 'OC No Recepcionada' &&
                            <div className=''>

                                <Steps
                                    progressDot
                                    status='error'
                                    current={1}
                                    items={[

                                        {
                                            title: 'OC Cancelada',
                                        },

                                    ]}
                                />
                            </div>
                        }
                        {
                            record.estado !== 'OC Cancelada' && record.estado !== 'OC No Recepcionada' &&
                            <Steps
                                progressDot
                                current={step}
                                items={[
                                    {
                                        title: 'Ticket Recibido',


                                    },
                                    {
                                        title: 'Liberada Depto',
                                        description: 'Solicitante',
                                    },
                                    {
                                        title: 'Liberada Finanzas',
                                    },
                                    {
                                        title: 'OC Enviada',

                                    },
                                    {
                                        title: 'OC Parcial',
                                    },
                                    {
                                        title: 'OC Recepcionada',
                                    },
                                ]}
                            />
                        }
                    </div>
                </Modal>
            }
        </div>
    )
}

export default Cart