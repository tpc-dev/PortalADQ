import { Breadcrumb } from 'antd'
import React, { useState } from 'react'
import ReemplazosData from '../../../data/Reemplazos.json'
import { normalizeText } from '../../../utils/paragraph'
import { isNotEmpty } from '../../../utils/validations'
import Search from './Search'
import Table from '../../Template/Table'
import Create from './Create'
import Actions from './Actions'
import useAuthContext from '../../../hooks/useAuthContext'
import useRemplazos from '../../../hooks/useRemplazos'
import { render } from 'react-dom'

function Remplazos() {


    const { user } = useAuthContext()

    const [search, setSearch] = useState({
        data: [],
        user: ''
    })

    const { data, isLoading, isSuccess, refetch } = useRemplazos()


    const handleSearch = e => {
        const user = normalizeText(e.target.value)
        const result = ReemplazosData.filter(u => normalizeText(u.vacations).includes(user) || normalizeText(u.replacer).includes(user) || normalizeText(u.comment).includes(user) || normalizeText(u.end_date).includes(user))
        setSearch({
            data: result,
            user
        })
    }

    const columns = [
        {
            title: 'En Vacaciones', dataIndex: 'id_Usuario_Vacaciones', key: 'id_Usuario_Vacaciones', align: 'left',
        },
        {
            title: 'Reemplazante', dataIndex: 'id_Usuario_Reemplazante', key: 'id_Usuario_Reemplazante', align: 'left',
        },
        {
            title: 'Comentario', dataIndex: 'comentario', key: 'comentario', align: 'center',
        },
        {
            title: 'Fecha Retorno', dataIndex: 'fecha_Retorno', key: 'fecha_Retorno', align: 'center', render: (text, record) => {
                return (
                    <div>
                        {new Date(record.fecha_Retorno).toLocaleDateString()}
                    </div>
                )
            }
        }
    ]

    if (user.isAdmin) {
        columns.push({
            title: 'Acciones', key: 'edit', align: 'center', render: (text, record) => <Actions Reemplazo={record} refetch={refetch} />
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
                        title: 'Remplazos',
                    }
                ]}
                className='text-sm mb-4'
            />

            <div className='flex justify-between flex-col lg:flex-row items-center mb-4'>
                <p className='font-semibold text-lg leading-none'>
                    Reemplazos
                </p>
            </div>
            <div className="flex flex-col lg:flex-row items-center gap-2 mb-4">
                <div className="flex-1 order-1">
                </div>
                <div className="flex gap-x-2 order-2">
                    {
                        user.isAdmin &&
                        <Create refetch={refetch} />
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

export default Remplazos