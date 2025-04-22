import React, { useEffect, useState } from 'react'
import Layout from '../../../components/Template/Layout'
import { Breadcrumb, Table } from 'antd'
import useProviders from '../../../hooks/useProviders'
import RequestOC from '../../../service/RequestOc'
import { alertError, alertWarning } from '../../../utils/alert'
import { render } from 'react-dom'
import Enable from './Enable'
import Delete from './Delete'

function DetailOc({ location }) {

    const { data, isLoading, isSuccess, isError } = useProviders()

    const [OC, setOC] = useState([])

    const columns = [
        { title: 'Pos', dataIndex: 'posicion', key: 'posicion', align: 'left', responsive: ['md'] },
        { title: 'Cantidad', dataIndex: 'cantidad', key: 'cantidad', align: 'left', responsive: ['md'] },
        { title: 'Moneda', dataIndex: 'mon', key: 'mon', align: 'center', responsive: ['md'] },
        { title: 'Prc Neto', dataIndex: 'prcNeto', key: 'prcNeto', align: 'center', responsive: ['md'] },
        { title: 'Proveedor', dataIndex: 'proveedor', key: 'proveedor', align: 'center', responsive: ['md'] },
        { title: 'Texto Breve', dataIndex: 'texto', key: 'texto', align: 'center', responsive: ['md'] },
        { title: 'Material', dataIndex: 'material', key: 'material', align: 'center', responsive: ['md'] },
        { title: 'Valor Neto', dataIndex: 'valorNeto', key: 'valorNeto', align: 'center', responsive: ['md'] },
        {
            title: 'Eliminar',
            align: 'center',
            responsive: ['md'],
            render: (record) => {
                return <Delete OC={record} />
            }
        },
        {
            title: 'Recepcionado',
            align: 'center',
            render: (record) => {
                return <Enable OC={record} />
            }
        }
    ]

    const getOC = () => {
        try {
            RequestOC.getOC(location.state?.ticket?.iD_Ticket || location.state?.ticket?.id_Ticket)
                .then(res => {
                    setOC(res)
                })
                .catch(e => {
                    alertWarning({ title: 'No hay OC asociadas' })
                })
        } catch (e) {
            console.log(e)
        }
    }

    useEffect(() => {
        getOC()
    }, [location.state?.ticket])

    return (
        <Layout>
            <Breadcrumb
                items={[
                    {
                        title: <a href="/solicitud">Inicio</a>,
                    },
                    {
                        title: <a href="/requests-oc">Solicitudes OC</a>,
                    },
                    {
                        title: `Confirmación recepción`,
                    },
                ]}
                className='text-sm mb-4'
            />
            <div className='w-full flex flex-col gap-5 items-start'>
                <h1 className='font-bold mb-3'>
                    Solicitud OC
                </h1>
                <div className='grid grid-cols-1 gap-3 w-full'>
                    <div className='grid grid-cols-2 justify-between gap-5 md:w-1/3'>
                        <p>N° Ticket:</p>
                        <div className='w-full'>
                            {/* Cambio: Se envolvió el número de ticket en un `div` para aplicar estilo y mejorar la presentación visual. - Felipe Guerra Blamey - 2025-04-18 */}
                            <p className="inline-block min-w-[180px] px-2">
                                {/* Cambio: Se aplicaron clases utilitarias de Tailwind CSS para destacar el número de ticket como texto subrayado e interactivo al pasar el cursor. Mejora la legibilidad e indica interactividad o importancia. - Felipe Guerra Blamey - 2025-04-18 */}
                                    <span className="text-sm font-medium tracking-wide text-black whitespace-nowrap underline decoration-2 decoration-blue-500 underline-offset-1 transition-colors duration-200 hover:decoration-blue-600 hover:text-black-700">
                                    {location.state?.ticket?.iD_Ticket || location.state?.ticket?.id_Ticket}
                                </span>
                            </p>
                        </div>
                    </div>

                    <div className='grid grid-cols-2 justify-between gap-5 md:w-1/3'>
                        <p>N ° Orden de compra:</p>
                        <div className='w-full'>
                            {/* Cambio: Se envolvió el número de orden de compra en un `div` y se aplicaron los mismos estilos visuales que al número de ticket para mantener consistencia visual. - Felipe Guerra Blamey - 2025-04-18 */}
                            <p className="inline-block min-w-[180px] px-2">
                                <span className="text-sm font-medium tracking-wide text-black whitespace-nowrap underline decoration-2 decoration-blue-500 underline-offset-1 transition-colors duration-200 hover:decoration-blue-600 hover:text-black-700">
                                    {location.state?.ticket?.numero_OC}
                                </span>
                            </p>
                        </div>
                    </div>
                </div>

                <h1 className='font-bold mb-3'>Proveedor</h1>
                <div className='grid grid-cols-1 gap-3 w-full'>
                    <div className='grid grid-cols-2 justify-between gap-5 md:w-1/3'>
                    {/* Cambio: Se aplicó el mismo estilo visual al campo "Detalle" para que coincida con los otros campos estilizados, asegurando una experiencia uniforme. - Felipe Guerra Blamey - 2025-04-18 */}
                        <p>Razon social:</p>
                        <p className="inline-block min-w-[180px] border-b-2 border-blue-500 pb-1 px-2 text-sm font-medium tracking-wide text-black whitespace-nowrap transition-colors duration-200 hover:border-blue-600 hover:text-black-700">
                            {data ? data.find(prov => prov.nombre_Fantasia === location.state?.ticket.proveedor ? location.state?.ticket.proveedor : location.state?.ticket.iD_Proveedor)?.razon_Social : 'Cargando...'}
                        </p>
                    </div>

                    <div className='grid grid-cols-2 justify-between gap-5 md:w-1/3'>
                        <p>Nombre Fantasia:</p>
                        {/* Cambio: Se aplicó el mismo estilo visual al campo "Detalle" para que coincida con los otros campos estilizados, asegurando una experiencia uniforme. - Felipe Guerra Blamey - 2025-04-18 */}
                        <p className="inline-block min-w-[180px] border-b-2 border-blue-500 pb-1 px-2 text-sm font-medium tracking-wide text-black whitespace-nowrap transition-colors duration-200 hover:border-blue-600 hover:text-black-700">
                            {location.state?.ticket?.iD_Proveedor || location.state?.ticket.proveedor}
                        </p>
                    </div>

                    <div className='grid grid-cols-2 justify-between gap-5 md:w-1/3'>
                        <p>Detalle:</p>
                        {/* Cambio: Se aplicó el mismo estilo visual al campo "Detalle" para que coincida con los otros campos estilizados, asegurando una experiencia uniforme. - Felipe Guerra Blamey - 2025-04-18 */}
                        <p className="inline-block min-w-[180px] border-b-2 border-blue-500 pb-1 px-2 text-sm font-medium tracking-wide text-black whitespace-nowrap transition-colors duration-200 hover:border-blue-600 hover:text-black-700">
                            {location.state?.ticket?.detalle ? location.state?.ticket?.detalle : 'Sin detalle'}
                        </p>
                    </div>
                </div>

                <h1 className='font-bold my-5'>Partes</h1>
                <Table
                    className='w-full'
                    dataSource={OC}
                    columns={columns}
                    loading={isLoading}
                />
            </div>
        </Layout>
    )
}

export default DetailOc
