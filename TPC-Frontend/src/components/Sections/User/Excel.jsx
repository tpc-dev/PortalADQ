import { Button, Form, Modal, Upload } from 'antd'
import React, { useState } from 'react'
import { AiFillFileExcel } from 'react-icons/ai'
import { DownloadOutlined, LoadingOutlined, UploadOutlined } from '@ant-design/icons'
import Excels from '../../../service/Excel'

function Excel() {

    const [modal, setModal] = useState(false)
    const [loading, setLoading] = useState(false)
    const [file, setFile] = useState([])

    const downloadExcel = () => {
        setLoading(true)
        try {
            Excels.Users()
                .then(response => {

                    setLoading(false)
                    //downloadFile type 
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(response);
                    link.download = 'Usuarios.xlsx';
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
                    <AiFillFileExcel size={20} color='green' />
                }
            </Button>
            <Modal
                open={modal}
                title="Usuarios"
                centered
                zIndex={3000}
                closable={true}
                destroyOnClose={true}
                maskClosable={false}
                keyboard={false}
                onCancel={() => setModal(false)}
                footer={null}
            >

                <Form name="import" preserve={false} className='flex flex-col gap-5'>

                    <Button icon={<DownloadOutlined />} onClick={() => downloadExcel()} block>
                        Descargar Excel

                    </Button>
                </Form>
            </Modal>
        </div>
    )
}

export default Excel