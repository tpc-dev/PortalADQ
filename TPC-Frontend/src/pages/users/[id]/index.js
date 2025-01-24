import React, { useEffect, useState } from 'react'
import Layout from '../../../components/Template/Layout'
import { Breadcrumb } from 'antd'
import User from '../../../service/User'
import { ImUserTie } from 'react-icons/im'

function DetalleUsuario({ location }) {

    const [user, setUser] = useState({})

    useEffect(() => {
        User.get(location?.state?.id_Usuario)
            .then(response => {
                setUser(response)
                console.log(response)
            })
            .catch(error => {
                console.log(error)
            })
    }, [])

    return (
        <Layout>
            <Breadcrumb
                items={[
                    {
                        title: <a href="/solicitud">Inicio</a>,
                    },
                    {
                        title: <a href="/users">
                            Lista de usuarios
                        </a>,
                    },
                    {
                        title: `Usuario ${location?.state?.id_Usuario} `,
                    },
                ]}
                className='text-sm mb-4'
            />
            {
                location?.state?.id_Usuario && (

                    <div className='flex flex-col items-center justify-center'>
                        <div className='flex flex-col justify-center items-center gap-2'>
                            <div className='border-2 border-[#1135A6] rounded-full p-2' >
                                <ImUserTie size={40} color='#1135A6' />
                            </div>
                            <h1>
                                Detalle Usuario {location?.state?.id_Usuario}
                            </h1>
                            <div className='flex flex-col gap-3 mt-10'>
                                <p className='grid grid-cols-2'>
                                    <span className='font-bold mr-3'>
                                        Nombre:
                                    </span>
                                    {user.nombre_Usuario} {user.apellido_paterno} {user.apellido_materno}
                                </p>
                                <p className='grid grid-cols-2'>
                                    <span className='font-bold mr-3'>
                                        Rut:
                                    </span>
                                    {user.rut_Usuario}
                                </p>
                                <p className='grid grid-cols-2'>
                                    <span className='font-bold mr-3'>
                                        Correo:
                                    </span>
                                    {user.correo_Usuario}
                                </p>
                                <p className='grid grid-cols-2'>
                                    <span className='font-bold mr-3'>
                                        Rol:
                                    </span>
                                    {user.admin == true ? 'Administrador' : 'Usuario'}
                                </p>

                                <p className='grid grid-cols-2'>
                                    <span className='font-bold mr-3'>
                                        Departamento:
                                    </span>
                                    <div className='flex flex-col '>

                                        {
                                            user.listaDepartamento?.map((item, index) => (
                                                <span className='list-item' key={index}>
                                                    {item}
                                                </span>
                                            ))
                                        }
                                    </div>
                                </p>



                            </div>
                        </div>
                    </div>
                )
            }

        </Layout>
    )
}

export default DetalleUsuario