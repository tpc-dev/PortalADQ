import { Breadcrumb, Button } from 'antd'
import React, { useState } from 'react'
import Table from '../../Template/Table'
import Search from './Search'
import { normalizeText } from '../../../utils/paragraph'
import { isNotEmpty } from '../../../utils/validations'
import useAuthContext from '../../../hooks/useAuthContext'
import Editar from './Editar'
import useLiberadores from '../../../hooks/useLiberadores'
import Create from './Create'

function Liberadores() {

    const { user } = useAuthContext()


    const { data, isLoading, isSuccess, refetch } = useLiberadores()

    const [search, setSearch] = useState({
        data: [],
        user: ''
    })




    const handleSearch = e => {
        const user = normalizeText(e.target.value)
        const result = data.filter(t => normalizeText(t.nombre_Usuario).includes(user) || normalizeText(t.nombre_Departamento).includes(user))
        setSearch({
            data: result,
            user
        })
    }


    const columns = [

        {
            title: 'Nombre', dataIndex: 'nombre_Usuario', key: 'name', align: 'left', responsive: ['md'],
        },

        { title: 'Nombre Departamento', dataIndex: 'nombre_Departamento', key: 'nombre_Departamento', align: 'left', responsive: ['md'] },


    ]

    if (user.isAdmin) {
        columns.push({
            title: 'Cambiar Liberador', dataIndex: 'actions', key: 'actions', align: 'center', responsive: ['md'],
            render: (text, record) => (
                <div className='flex justify-center items-center gap-2'>
                    <Editar liberador={record} refetch={refetch} />
                </div>
            )
        })
    }

    return (
        <div>
            <Breadcrumb
                items={[
                    {
                        title: <a href="/solicitud">Inicio</a>,
                    },
                    {
                        title: 'Administracion',
                    },
                    {
                        title: 'Gestion de Liberadores',
                    }
                ]}
                className='text-sm mb-4'
            />
            <div className='flex justify-between flex-col lg:flex-row items-center mb-4'>
                <p className='font-semibold text-lg leading-none'>
                    Liberadores
                </p>
            </div>
            <div className="flex flex-col lg:flex-row items-center gap-2 mb-4">
                <div className="flex-1 order-1">
                </div>
                <div className="flex gap-x-2 order-2">
                    {
                        user.isAdmin &&
                        <>
                            <Create refetch={refetch} />

                        </>
                    }
                </div>
                <Search onChange={handleSearch} />
            </div>

            <Table
                columns={columns}
                data={isSuccess ? (isNotEmpty(search.user) ? search.data : data) : []}
            />
        </div>
    )
}

export default Liberadores