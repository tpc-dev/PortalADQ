import { Breadcrumb } from 'antd'
import React, { useEffect, useState } from 'react'
import Layout from '../../../components/Template/Layout'
import Tickets from '../../../data/TicketsOc.json'
import { IoDocumentsOutline } from "react-icons/io5";


function RequestOcDetail({ id }) {

    const [request, setRequest] = useState({})

    const SearchRequest = (id) => {
        const search = Tickets.find(request => request.id == id)
        setRequest(search)
    }

    useEffect(() => {
        SearchRequest(id)
    }, [])


    return (
        <Layout>
            <Breadcrumb
                items={[
                    {
                        title: <a href="/solicitud">Inicio</a>,
                    },
                    {
                        title: <a href="/requests-oc">
                            Solicitudes OC
                        </a>,
                    },
                    {
                        title: `Detalle de solicitud`,
                    },
                ]}
                className='text-sm mb-4'
            />

            <div className='flex flex-col justify-center items-center'>
                <div className='flex flex-col justify-center items-center gap-2'>
                    <div className='border-2 border-[#1135A6] rounded-full p-2' >
                        <IoDocumentsOutline size={40} color='#1135A6' />
                    </div>
                    <h1>
                        Detalle de OC N°{request.id}
                    </h1>
                    <p>
                        Fecha de creación: {request.fecha}
                    </p>
                </div>
                <div className='grid grid-cols-1 md:grid-cols-2 w-full gap-10 mt-10'>

                    <div className='flex flex-col  gap-2 border shadow-md p-5'>
                        <div className='flex flex-col gap-3'>
                            <p>
                                <span className='font-bold mr-3'>
                                    Nº Ticket:
                                </span>
                                {request.ticket}
                            </p>
                            <p>
                                <span className='font-bold mr-3'>
                                    Nº Solped:
                                </span>
                                {request.solped}
                            </p>
                            <p>
                                <span className='font-bold mr-3'>
                                    Nº OC:
                                </span>
                                {request.oc}
                            </p>
                            <p>
                                <span className='font-bold mr-3'>
                                    Tipo de solicitud:
                                </span>
                                {request.type}
                            </p>
                            <p>
                                <span className='font-bold mr-3'>
                                    Estado:
                                </span>
                                {request.status}
                            </p>

                            <p>
                                <span className='font-bold mr-3'>
                                    Fecha:
                                </span>
                                {request.fecha}
                            </p>
                        </div>
                    </div>

                    <div className='flex flex-col gap-2 border rounded-md p-5 shadow-md'>
                        <p>
                            <span className='font-bold mr-3'>
                                Proveedor:
                            </span>
                            {request.provider}
                        </p>

                        <p>
                            <span className='font-bold mr-3'>
                                Detalle:
                            </span>
                            {request.detalle}
                        </p>

                        <p>
                            <span className='font-bold mr-3'>
                                Solicitante:
                            </span>
                            {request.solicitante}
                        </p>

                        <p>
                            <span className='font-bold mr-3'>
                                Orden estadistica:
                            </span>
                            {request.orden_estadistica}
                        </p>

                        <p>
                            <span className='font-bold mr-3'>
                                Razon social:
                            </span>
                            {request.razon_social}
                        </p>

                        <p>
                            <span className='font-bold mr-3'>
                                Liberador Financiero:
                            </span>
                            {request.liberador_financiero}
                        </p>

                        <p>
                            <span className='font-bold mr-3'>
                                Liberador Departamendo:
                            </span>
                            {request.liberador_departamento}
                        </p>

                    </div>
                </div>

            </div>

        </Layout >
    )
}

export default RequestOcDetail