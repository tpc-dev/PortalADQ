import { Button, Modal } from 'antd'
import React, { useState } from 'react'
import { DownloadOutlined, LoadingOutlined } from '@ant-design/icons'
import Excels from '../../../service/Excel'
import { IoDownload } from 'react-icons/io5'
import { MdDownload } from 'react-icons/md'

function Descargar() {

    const [modal, setModal] = useState(false)
    const [loading, setLoading] = useState(false)


    const downloadExcel = () => {
        setLoading(true)
        try {
            Excels.OC()
                .then(response => {
                    console.log(response)
                    setLoading(false)
                    //download blob type
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(response);
                    link.download = 'OrdenesCompra.xlsx';
                    link.click();
                })
                .catch(error => {
                    console.log(error)
                    setLoading(false)
                })

        } catch (e) {
            console.log(e)
            setLoading(false)
        }
    }

    return (
        <div>

            <Button
                className="px-5"
                onClick={() => setModal(true)}
            >
                {loading ? <LoadingOutlined /> :
                    <MdDownload size={20} color='green' />
                }
            </Button>
            <Modal
                open={modal}
                title="Descargar Ordenes de Compra"
                centered
                zIndex={3000}
                closable={true}
                destroyOnClose={true}
                maskClosable={false}
                keyboard={false}
                onCancel={() => setModal(false)}
                footer={null}
            >

                <Button className="w-full" icon={<DownloadOutlined />} block onClick={() => downloadExcel()}>Descargar</Button>


            </Modal>
        </div>
    )
}

export default Descargar