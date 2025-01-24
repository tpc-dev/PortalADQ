import { Alert, Button, Form, Modal, Upload } from 'antd'
import React, { useState } from 'react'
import { AiFillFileExcel } from 'react-icons/ai'
import { DownloadOutlined, LoadingOutlined, UploadOutlined } from '@ant-design/icons'
import Excels from '../../../service/Excel'
import { alertError, alertSuccess } from '../../../utils/alert'
import { IoReload } from 'react-icons/io5'

function Excel() {

    const [modal, setModal] = useState(false)
    const [loading, setLoading] = useState(false)
    const [file, setFile] = useState([])


    const uploadExcel = () => {
        setLoading(true)
        try {
            const formData = new FormData()
            formData.append('file', file[0])

            //solo archivos excel xlsx 
            if (file[0].type !== 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {
                setLoading(false)
                formData.delete('file')
                return alertError({ message: 'El archivo debe ser un excel', description: 'Por favor, suba un archivo excel' })
            }

            Excels.OCA({ formData })
                .then(response => {

                    setLoading(false)
                    alertSuccess({ message: 'Ordenes de compra actualizadas correctamente' })
                })
                .catch(error => {

                    setLoading(false)
                    alertError({ message: 'Error al actualizar ordenes de compra' })
                })

        } catch (e) {
            alertError({ message: 'Error al actualizar ordenes de compra' })
            setLoading(false)
        }
    }


    const propsUpload = {
        maxCount: 1,
        onRemove: file => setFile([]),
        beforeUpload: file => {
            setFile([file])
            return false
        },
        fileList: file
    }

    return (
        <div>

            <Button
                className="px-5"
                onClick={() => setModal(true)}
            >
                {loading ? <LoadingOutlined /> :
                    <IoReload size={20} color='green' />
                }
            </Button>
            <Modal
                open={modal}
                title="Actualizar Ordenes de Compra"
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

                    <Upload accept=' '  {...propsUpload} className="w-full">
                        <Button icon={<UploadOutlined />} block>Cargar archivo</Button>
                    </Upload>


                    <Button
                        type="primary"
                        onClick={() => uploadExcel()}
                        block
                        loading={loading}
                        disabled={file.length === 0}
                    >
                        Subir Excel
                    </Button>

                </Form>

            </Modal>
        </div>
    )
}

export default Excel