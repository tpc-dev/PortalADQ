import { Button, Form, Modal, Upload } from 'antd'
import React, { useState } from 'react'
import { AiFillFileExcel } from 'react-icons/ai'
import { DownloadOutlined, LoadingOutlined, UploadOutlined } from '@ant-design/icons'
import Excels from '../../../service/Excel'
import { alertError } from '../../../utils/alert'

function Excel() {

    const [modal, setModal] = useState(false)
    const [loading, setLoading] = useState(false)
    const [file, setFile] = useState([])

    const uploadExcel = () => {
        setLoading(true)
        try {
            const formData = new FormData()
            formData.append('file', file[0])

            if (file[0].type !== 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {
                setLoading(false)
                formData.delete('file')
                return alertError({ message: 'El archivo debe ser un excel', description: 'Por favor, suba un archivo excel' })
            }

            Excels.CostCenter({ FormData })
                .then(response => {
                    setLoading(false)
                    console.log(response)
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
                    <AiFillFileExcel size={20} color='green' />
                }
            </Button>
            <Modal
                open={modal}
                title="Importar Centro de Costos"
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

                    <Upload  {...propsUpload} className="w-full">
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