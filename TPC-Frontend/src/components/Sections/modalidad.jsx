
import { Select, Button, Radio, Space, Input, Upload } from 'antd'
import Dragger from 'antd/es/upload/Dragger'
import React, { useState } from 'react'
import { InboxOutlined } from '@ant-design/icons'
import useBienServicio from '../../hooks/useBienServicio'
import useProviders from '../../hooks/useProviders'
import useOrdenesEstadisticas from '../../hooks/useOrdenesEstadisticas'
import Archivo from '../../service/Archivo'

function Modalidad({ Form, nextStep, setModalidadState, modalidadState, tipoSolicitud, previousStep, onFinish, setFile }) { 
    const BienServiciodata = useBienServicio(); 
    const Providers = useProviders(); 
    const OrdenesEstadisticas = useOrdenesEstadisticas(); 
    const handleModalidad = ({ target }) => { const value = target.value; setModalidadState(value); 
    };
 const handleFileChange = (e) => { 
    const file = e.target.files[0]; setFile(file); 
};
 return (
        <div>
            <div className="flex flex-col gap-3 sm:gap-5 my-8 w-full">

                <div className="flex flex-col gap-5 sm:gap-4 items-start w-1/2">
                    <label>Tipo de modalidad</label>
                    <div className='flex items-center justify-center'>
                        <Form.Item name="modalidad" rules={[{ required: true, message: 'Por favor seleccione una modalidad' }]} className=' w-2/2 mb-0 flex gap-3'>
                            <Radio.Group onChange={handleModalidad}>
                                <Space direction="horizontal">
                                    <Radio value={1}>Con solped</Radio>
                                    <Radio value={2}>Sin Solped</Radio>
                                </Space>
                            </Radio.Group>
                        </Form.Item>
                    </div>
                    


                    
                    {
                        tipoSolicitud === 1 && modalidadState === 1 &&
                        <>
                        <div className='grid grid-cols-2 w-full items-center'>
                            <p>
                                Número de solped
                            </p>
                            <Form.Item name="solped" rules={[{ required: true, message: 'Por favor ingrese el número de solped' }]} className='mb-0'>
                                <Input placeholder="Número de solped" />
                            </Form.Item>
                        </div>
                            <div className='grid grid-cols-2 w-full items-center'>
                                <p>
                                    Proveedor
                                </p>
                                <Form.Item name="proveedor" rules={[{ required: true, message: 'Por favor ingrese el proveedor' }]} className='mb-0'>
                                    <Select placeholder="Proveedor" >
                                        {
                                            Providers.data?.map((item, index) => (
                                                <Select.Option key={index} value={item.iD_Proveedores}>{item.razon_Social}</Select.Option>
                                            ))
                                        }
                                    </Select>
                                </Form.Item>
                            </div>

                            <div className='grid grid-cols-2 w-full items-center'>
                                <p>
                                    Orden estadistica
                                </p>
                                <Form.Item name="ordenEstadistica" rules={[{ required: true, message: 'Por favor ingrese la orden estadistica' }]} className='mb-0'>
                                    <Select placeholder="Orden estadistica" >
                                        {
                                            OrdenesEstadisticas.data?.map((item, index) => (
                                                <Select.Option key={index} value={item.id_Orden_Estadistica}>{item.codigo_OE +" "+item.nombre}</Select.Option>
                                            ))
                                        }
                                    </Select>
                                </Form.Item>
                            </div>
                            <div>
                                    <p>Archivo</p> 
                            <Form.Item name="archivo" rules={[{ required: true, message: 'Por favor ingrese el archivo' }]} className='mb-10'>
                            <Input type='file' onChange={handleFileChange} className="h-10" /> 
                            </Form.Item>
                                    </div>
                        </>
                    }
                                        {
                        modalidadState === 1 &&    tipoSolicitud === 2 && 
                        <>
                        <div className='grid grid-cols-2 w-full items-center'>
                            <p>
                                Número de solped
                            </p>
                            <Form.Item name="solped" rules={[{ required: true, message: 'Por favor ingrese el número de solped' }]} className='mb-0'>
                                <Input placeholder="Número de solped" />
                            </Form.Item>
                        </div>
                        <div>
                                    <p>Archivo</p> 
                            <Form.Item name="archivo" rules={[{ required: true, message: 'Por favor ingrese el archivo' }]} className='mb-10'>
                            <Input type='file' onChange={handleFileChange} className="h-10" /> 
                            </Form.Item>
                                    </div>


                        </>
                    }
                    
                    {
                        modalidadState && modalidadState === 2 &&
                        <div className='flex flex-col gap-2'>
                            <div className='grid grid-cols-2 w-full items-center'>
                                <p>
                                    Detalle
                                </p>
                                <Form.Item name="detalle" rules={[{ required: true, message: 'Por favor ingrese el detalle' }]} className='mb-0'>

                                    <Input.TextArea placeholder="Detalle" />
                                </Form.Item>
                            </div>

                            {/* //Cotizar sin solped  */}
                            {
                                tipoSolicitud === 2 &&
                                <>
                                <div className='grid grid-cols-2 w-full items-center'>
                                    <p>
                                        Bien o servicio
                                    </p>
                                    <Form.Item name="bienServicio" rules={[{ required: true, message: 'Por favor ingrese el bien o servicio' }]} className='mb-0'>
                                        <Select placeholder="Bien o servicio" >
                                            {
                                                BienServiciodata.data?.map((item, index) => (
                                                    <Select.Option key={index} value={item.iD_Bien_Servicio}>{item.bien_Servicio}</Select.Option>
                                                ))
                                            }
                                        </Select>
                                    </Form.Item>
                                </div>
                                <div>
                                <p>Archivo</p> 
                            <Form.Item name="archivo" rules={[{ required: true, message: 'Por favor ingrese el archivo' }]} className='mb-10'>
                            <Input type='file' onChange={handleFileChange} className="h-10" /> 
                            </Form.Item>
                            </div></>
                            }
  

                            {/* Orden sin solped  */}
                            {
                                tipoSolicitud === 1 &&
                                <>
                                    <div className='grid grid-cols-2 w-full items-center'>
                                        <p>
                                            Orden estadistica
                                        </p>
                                        <Form.Item name="ordenEstadistica" rules={[{ required: true, message: 'Por favor ingrese la orden estadistica' }]} className='mb-0'>
                                            <Select placeholder="Orden estadistica" >
                                                {
                                                    OrdenesEstadisticas.data?.map((item, index) => (
                                                        <Select.Option key={index} value={item.id_Orden_Estadistica}>{item.codigo_OE+" "+item.nombre}</Select.Option>
                                                    ))
                                                }
                                            </Select>
                                        </Form.Item>
                                    </div>
                                    <div className='grid grid-cols-2 w-full items-center'>
                                        <p>
                                            Proveedor
                                        </p>
                                        <Form.Item name="proveedor" rules={[{ required: true, message: 'Por favor ingrese el proveedor' }]} className='mb-0'>
                                            <Select placeholder="Proveedor" >
                                                {
                                                    Providers.data?.map((item, index) => (
                                                        <Select.Option key={index} value={item.iD_Proveedores}>{item.razon_Social}</Select.Option>
                                                    ))
                                                }
                                            </Select>
                                        </Form.Item>
                                    </div>
                                    <div>
                                    <p>Archivo</p> 
                            <Form.Item name="archivo" rules={[{ required: true, message: 'Por favor ingrese el archivo' }]} className='mb-10'>
                            <Input type='file' onChange={handleFileChange} className="h-10" /> 
                            </Form.Item>
                                    </div>
                                    
                                    {/* <div className='grid grid-cols-2 w-full items-center'>
                                        <p>
                                            Otros Archivos
                                        </p>
                                        <Form.Item name="other_archives" className='mb-0'>
                                            <Dragger>
                                                <p className="ant-upload-drag-icon">
                                                    <InboxOutlined size={10} />
                                                </p>
                                            </Dragger>
                                        </Form.Item>
                                    </div> */}
                                </>
                            }


                        </div>
                    }
                </div>
            </div>
            <div className='w-full flex justify-between items-center'>
                <Button onClick={previousStep} type="primary">Atras</Button>
                <Button onClick={() => onFinish()} type="primary">Siguiente</Button>
            </div>

        </div>
    )
}

export default Modalidad