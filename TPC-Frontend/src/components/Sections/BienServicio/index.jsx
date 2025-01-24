import { Breadcrumb, Table } from 'antd'
import React, { useState } from 'react'
import useAuthContext from '../../../hooks/useAuthContext'
import useBienServicio from '../../../hooks/useBienServicio'
import { normalizeText } from '../../../utils/paragraph'
import { isNotEmpty } from '../../../utils/validations'
import Search from './Search'
import Create from './Create'
import Actions from './Actions'
import Excel from './Excel'

function BienServicio() {

    const { user } = useAuthContext()

    const { data, isSuccess, refetch } = useBienServicio()

    const [search, setSearch] = useState({
        data: [],
        BienServicio: ''
    })

    const handleSearch = e => {
        const BienServicio = normalizeText(e.target.value)
        const result = data.filter(u => normalizeText(u.iD_Bien_Servicio.toString()).includes(BienServicio) || normalizeText(u.bien_Servicio).includes(BienServicio))

        setSearch({
            data: result,
            BienServicio
        })
    }

    const columns = [
        { title: 'ID', dataIndex: 'iD_Bien_Servicio', key: 'iD_Bien_Servicio', align: 'left', responsive: ['md'] },
        { title: 'Nombre', dataIndex: 'bien_Servicio', key: 'bien_Servicio', align: 'left', responsive: ['md'] },

    ]

    if (user.isAdmin) {
        columns.push({
            title: 'Acciones',
            dataIndex: 'acciones',
            key: 'acciones',
            align: 'center',
            render: (text, record) => (
                <Actions bienServicio={record} refetch={refetch} />
            ),
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
                        title: 'Bien y Servicio',
                    },
                ]}
                className='text-sm mb-4'
            />
            <div className='flex flex-col mb-4'>
                <p className='font-semibold text-lg leading-none'>
                    Bien y Servicio
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
                            {/* <Excel /> */}
                        </>
                    }
                </div>
                <Search onChange={handleSearch} />
            </div>
            <Table
                columns={columns}
                dataSource={isSuccess ? (isNotEmpty(search.BienServicio) ? search.data : data) : []}
                rowKey='id_Ceco'
                pagination={
                    {
                        defaultPageSize: 10,
                        showSizeChanger: false,
                    }
                }
            />
        </div>
    )
}

export default BienServicio