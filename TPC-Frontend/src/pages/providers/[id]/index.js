import React, { useEffect, useState } from 'react'
import Layout from '../../../components/Template/Layout'
import { Breadcrumb } from 'antd'
import { IoDocumentsOutline } from 'react-icons/io5'
import { ImUserTie } from 'react-icons/im'


function ProviderDetail({ location }) {

    console.log(location)

    return (
        <Layout>
            <Breadcrumb
                items={[
                    {
                        title: <a href="/solicitud">Inicio</a>,
                    },
                    {
                        title: <a href="/location.state.iD_Proveedoress">
                            Lista de proveedores
                        </a>,
                    },
                    {
                        title: `Proveedor ${location?.state?.iD_Proveedores}`,
                    },
                ]}
                className='text-sm mb-4'
            />
            {
                location?.state?.iD_Proveedores && (

                    <div className='flex flex-col items-center justify-center'>
                        <div className='flex flex-col justify-center items-center gap-2'>
                            <div className='border-2 border-[#1135A6] rounded-full p-2' >
                                <ImUserTie size={40} color='#1135A6' />
                            </div>
                            <h1>
                                Ficha Proveedor {location?.state?.iD_Proveedores}
                            </h1>
                        </div>
                        <div className='grid grid-cols-2  gap-10 mt-10'>
                            <div className='flex flex-col gap-2 border shadow-md p-5 px-10 items-start'>
                                <h1 className='text-center self-center'>
                                    Detalles del Proveedor
                                </h1>
                                <div className='flex flex-col gap-3'>
                                    <p>
                                        <span className='font-bold mr-3'>
                                            Rut:
                                        </span>
                                        {location.state.rut_Proveedor}
                                    </p>
                                    <p>
                                        <span className='font-bold mr-3'>
                                            Razón Social:
                                        </span>
                                        {location.state.razon_Social}
                                    </p>
                                    <p>
                                        <span className='font-bold mr-3'>
                                            Nombre Fantasia:
                                        </span>
                                        {location.state.nombre_Fantasia}
                                    </p>
                                    <p>
                                        <span className='font-bold mr-3'>
                                            Dirección:
                                        </span>
                                        {location.state.direccion}
                                    </p>
                                    <p>
                                        <span className='font-bold mr-3'>
                                            Comuna:
                                        </span>
                                        {location.state.comuna}
                                    </p>
                                    <p>
                                        <span className='font-bold mr-3'>
                                            N° Cuenta:
                                        </span>
                                        {location.state.n_Cuenta}
                                    </p>
                                    <p>
                                        <span className='font-bold mr-3'>
                                            Swift:
                                        </span>
                                        {location.state.swift}
                                    </p>
                                </div>
                            </div>
                            <div className='flex flex-col gap-2 border shadow-md p-5 px-10 items-start'>
                                <h1 className='text-center self-center'>
                                    Detalles del Representante
                                </h1>
                                <div className='flex flex-col gap-3'>
                                    <p>

                                        <span className='font-bold mr-3'>
                                            Nombre:
                                        </span>
                                        {location.state.nombre_Representante}

                                    </p>
                                    <p>
                                        <span className='font-bold mr-3'>
                                            Correo:
                                        </span>
                                        {location.state.email_Representante}
                                    </p>

                                    <p>
                                        <span className='font-bold mr-3'>
                                            Cargo:
                                        </span>
                                        {location.state.cargo_Representante}
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                )
            }

        </Layout>
    )
}

export default ProviderDetail