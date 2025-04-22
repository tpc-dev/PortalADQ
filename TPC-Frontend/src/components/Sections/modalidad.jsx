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
    const handleModalidad = ({ target }) => { const value = target.value; setModalidadState(value); };
    const [localFile, setLocalFile] = useState(null);

    const handleFileChange = (e) => { 
        const file = e.target.files[0]; setFile(file); 
        if (file) {
            setFile(file);
            console.log("Archivo seleccionado:", file); // Opcional: para depuración
        } else {
            setFile(null);
            console.log("No se seleccionó ningún archivo."); // Opcional: para depuración
        }
    };

    return (
        <div>
            <div className="flex flex-col gap-3 sm:gap-5 my-8 w-full">
                {/* Cambio: Se cambió la clase CSS de ancho de w-1/2 a w-2/2 para ajustar el ancho del contenedor principal. - Felipe Guerra Blamey - 2025-04-18 */}
                <div className="flex flex-col gap-5 sm:gap-4 items-start w-2/2">
                    <label>Tipo de modalidad</label>
                    <div className='flex items-center justify-center'>
                        <Form.Item
                            name="modalidad"
                            rules={[{ required: true, message: 'Por favor seleccione una modalidad' }]}
                            className='w-2/2 mb-0 flex gap-3'
                        >
                            <Radio.Group onChange={handleModalidad}>
                                <Space direction="horizontal">
                                    <Radio value={1}>Con Solped</Radio>
                                    <Radio value={2}>Sin Solped</Radio>
                                </Space>
                            </Radio.Group>
                        </Form.Item>
                    </div>

                    {tipoSolicitud === 1 && modalidadState === 1 && (
                        <>
                            {/* Cambio: Se modificó el layout de "Número de solped" de un grid a un flex con justify-between y se añadieron clases w-1/4 y w-3/4 para controlar el ancho de etiqueta y campo. - Felipe Guerra Blamey - 2025-04-18 */}
                            <div className='flex items-center w-full justify-between'>
                                <p className="w-1/4">Número de solped</p>
                                <Form.Item
                                    name="solped"
                                    rules={[{ required: true, message: 'Por favor ingrese el número de solped' }]}
                                    className='mb-0 w-3/4'
                                >
                                    <Input placeholder="Número de solped" />
                                </Form.Item>
                            </div>

                            {/* Cambio: Se reemplazó el grid por un flex en la sección "Proveedor" y se añadieron clases w-1/4 y w-3/4 para mejor control de ancho. - Felipe Guerra Blamey - 2025-04-18 */}
                            {/* Cambio: Se agregó funcionalidad de búsqueda en el Select con showSearch, optionsFilterProp y filterOption para filtrar proveedores. - Felipe Guerra Blamey - 2025-04-18 */}
                            <div className='flex items-center w-full justify-between'>
                                <p className="w-1/4">Proveedor</p>
                                <Form.Item
                                    name="proveedor"
                                    rules={[{ required: true, message: 'Por favor ingrese el proveedor' }]}
                                    className='mb-0 w-3/4'
                                >
                                    <Select
                                        placeholder="Proveedor"
                                        showSearch
                                        optionsFilterProp="children"
                                        filterOption={(input, option) =>
                                            option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                        }
                                    >
                                        {Providers.data?.map((item, index) => (
                                            <Select.Option key={index} value={item.iD_Proveedores}>
                                                {item.razon_Social}
                                            </Select.Option>
                                        ))}
                                    </Select>
                                </Form.Item>
                            </div>

                            {/* Cambio: Se cambió grid a flex en "Orden estadistica" con w-1/4 y w-3/4, y se añadió búsqueda en Select. - Felipe Guerra Blamey - 2025-04-18 */}
                            <div className='flex items-center w-full justify-between'>
                                <p className="w-1/4">Orden estadistica</p>
                                <Form.Item
                                    name="ordenEstadistica"
                                    rules={[{ required: true, message: 'Por favor ingrese la orden estadistica' }]}
                                    className='mb-0 w-3/4'
                                >
                                    <Select
                                        placeholder="Orden estadistica"
                                        showSearch
                                        optionsFilterProp="children"
                                        filterOption={(input, option) =>
                                            option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                        }
                                    >
                                        {OrdenesEstadisticas.data?.map((item, index) => (
                                            <Select.Option key={index} value={item.id_Orden_Estadistica}>
                                                {item.codigo_OE + " " + item.nombre}
                                            </Select.Option>
                                        ))}
                                    </Select>
                                </Form.Item>
                            </div>

                            {/* Cambio: Se cambió grid a flex en "Archivo" con w-1/4 y w-3/4. - Felipe Guerra Blamey - 2025-04-18 */}
                            <div className='flex items-center w-full justify-between'>
                                <p className="w-1/4">Archivo</p>
                                <Form.Item
                                    name="archivo"
                                    rules={[{ required: true, message: 'Por favor ingrese el archivo' }]}
                                    className='mb-0 w-3/4'
                                >
                                    <Input type='file' onChange={handleFileChange} className="h-10" />
                                </Form.Item>
                            </div>
                        </>
                    )}

                    {modalidadState === 1 && tipoSolicitud === 2 && (
                        <>
                            {/* Cambio: Se modificó "Número de solped" a layout flex con w-1/4 y w-3/4 en lugar de grid. - Felipe Guerra Blamey - 2025-04-18 */}
                            <div className='flex items-center w-full justify-between'>
                                <p className="w-1/4">Número de solped</p>
                                <Form.Item
                                    name="solped"
                                    rules={[{ required: true, message: 'Por favor ingrese el número de solped' }]}
                                    className='mb-0 w-3/4'
                                >
                                    <Input placeholder="Número de solped" />
                                </Form.Item>
                            </div>

                            {/* Cambio: Se modificó "Archivo" a layout flex con w-1/4 y w-3/4 en lugar de grid. - Felipe Guerra Blamey - 2025-04-18 */}
                            <div className='flex items-center w-full justify-between'>
                                <p className="w-1/4">Archivo</p>
                                <Form.Item
                                    name="archivo"
                                    rules={[{ required: true, message: 'Por favor ingrese el archivo' }]}
                                    className='mb-0 w-3/4'
                                >
                                    <Input type='file' onChange={handleFileChange} className="h-10" />
                                </Form.Item>
                            </div>
                        </>
                    )}

                    {modalidadState === 2 && (
                        <>
                            {/* Cambio: Se modificó "Detalle" a layout flex con w-1/4 y w-3/4 en lugar de grid. - Felipe Guerra Blamey - 2025-04-18 */}
                            <div className='flex items-center w-full justify-between'>
                                <p className="w-1/4">Detalle</p>
                                <Form.Item
                                    name="detalle"
                                    rules={[{ required: true, message: 'Por favor ingrese el detalle' }]}
                                    className='mb-0 w-3/4'
                                >
                                    <Input.TextArea placeholder="Detalle" />
                                </Form.Item>
                            </div>

                            {/* Cotizar sin solped  */}
                            {tipoSolicitud === 2 && (
                                <>
                                    {/* Cambio: Se modificó "Bien o servicio" a layout flex con w-1/4 y w-3/4 y se agregó showSearch para búsqueda. - Felipe Guerra Blamey - 2025-04-18 */}
                                    <div className='flex items-center w-full justify-between'>
                                        <p className="w-1/4">Bien o servicio</p>
                                        <Form.Item
                                            name="bienServicio"
                                            rules={[{ required: true, message: 'Por favor ingrese el bien o servicio' }]}
                                            className='mb-0 w-3/4'
                                        >
                                            <Select
                                                placeholder="Bien o servicio"
                                                showSearch
                                                optionsFilterProp="children"
                                                filterOption={(input, option) =>
                                                    option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                                }
                                            >
                                                {BienServiciodata.data?.map((item, index) => (
                                                    <Select.Option key={index} value={item.iD_Bien_Servicio}>
                                                        {item.bien_Servicio}
                                                    </Select.Option>
                                                ))}
                                            </Select>
                                        </Form.Item>
                                    </div>
                                    {/* Cambio: Se modificó "Archivo" a layout flex con w-1/4 y w-3/4 en lugar de grid. - Felipe Guerra Blamey - 2025-04-18 */}
                                    <div className='flex items-center w-full justify-between'>
                                        <p className="w-1/4">Archivo</p>
                                        <Form.Item
                                            name="archivo"
                                            rules={[{ required: true, message: 'Por favor ingrese el archivo' }]}
                                            className='mb-0 w-3/4'
                                        >
                                            <Input type='file' onChange={handleFileChange} className="h-10" />
                                        </Form.Item>
                                    </div>
                                </>
                            )}

                            {/* Orden sin solped  */}
                            {tipoSolicitud === 1 && (
                                <>
                                    {/* Cambio: Se modificó "Orden estadistica" a layout flex con w-1/4 y w-3/4 y se añadió búsqueda en Select. - Felipe Guerra Blamey - 2025-04-18 */}
                                    <div className='flex items-center w-full justify-between'>
                                        <p className="w-1/4">Orden estadistica</p>
                                        <Form.Item
                                            name="ordenEstadistica"
                                            rules={[{ required: true, message: 'Por favor ingrese la orden estadistica' }]}
                                            className='mb-0 w-3/4'
                                        >
                                            <Select
                                                placeholder="Orden estadistica"
                                                showSearch
                                                optionsFilterProp="children"
                                                filterOption={(input, option) =>
                                                    option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                                }
                                            >
                                                {OrdenesEstadisticas.data?.map((item, index) => (
                                                    <Select.Option key={index} value={item.id_Orden_Estadistica}>
                                                        {item.codigo_OE + " " + item.nombre}
                                                    </Select.Option>
                                                ))}
                                            </Select>
                                        </Form.Item>
                                    </div>
                                    {/* Cambio: Se modificó "Proveedor" a layout flex con w-1/4 y w-3/4 y se añadió búsqueda en Select. - Felipe Guerra Blamey - 2025-04-18 */}
                                    <div className='flex items-center w-full justify-between'>
                                        <p className="w-1/4">Proveedor</p>
                                        <Form.Item
                                            name="proveedor"
                                            rules={[{ required: true, message: 'Por favor ingrese el proveedor' }]}
                                            className='mb-0 w-3/4'
                                        >
                                            <Select
                                                placeholder="Proveedor"
                                                showSearch
                                                optionsFilterProp="children"
                                                filterOption={(input, option) =>
                                                    option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                                }
                                            >
                                                {Providers.data?.map((item, index) => (
                                                    <Select.Option key={index} value={item.iD_Proveedores}>
                                                        {item.razon_Social}
                                                    </Select.Option>
                                                ))}
                                            </Select>
                                        </Form.Item>
                                    </div>
                                    {/* Cambio: Se modificó "Archivo" a layout flex con w-1/4 y w-3/4 en lugar de grid. - Felipe Guerra Blamey - 2025-04-18 */}
                                    <div className='flex items-center w-full justify-between'>
                                        <p className="w-1/4">Archivo</p>
                                        <Form.Item
                                            name="archivo"
                                            rules={[{ required: true, message: 'Por favor ingrese el archivo' }]}
                                            className='mb-0 w-3/4'
                                        >
                                            <Input type='file' onChange={handleFileChange} className="h-10" />
                                        </Form.Item>
                                    </div>
                                </>
                            )}
                        </>
                    )}
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
