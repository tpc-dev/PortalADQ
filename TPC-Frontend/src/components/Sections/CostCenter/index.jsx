import { Breadcrumb, Table } from 'antd'
import React, { useState } from 'react'
import CentroCostosData from '../../../data/CentroCostos.json'
import Create from './Create'
import Actions from './Actions'
import Search from './Search'
import { normalizeText } from '../../../utils/paragraph'
import { isNotEmpty } from '../../../utils/validations'
import useAuthContext from '../../../hooks/useAuthContext'
import useCentroCosto from '../../../hooks/useCentroCosto'
import Excel from './Excel'

function CentroDeCosto() {

    const { user } = useAuthContext()


    const { isSuccess, data, refetch } = useCentroCosto()

    const [search, setSearch] = useState({
        data: [],
        ceco: ''
    })

    const handleSearch = e => {
        const ceco = normalizeText(e.target.value)
        const result = data.filter(u => normalizeText(u.id_Ceco.toString()).includes(ceco) || normalizeText(u.nombre).includes(ceco) || normalizeText(u.codigo_Ceco.toString()).includes(ceco))

        setSearch({
            data: result,
            ceco
        })
    }

    const columns = [
        { title: 'Id', dataIndex: 'id_Ceco', key: 'id_Ceco', align: 'left', responsive: ['md'] },
        { title: 'Nombre', dataIndex: 'nombre', key: 'nombre', align: 'left', responsive: ['md'] },
        { title: 'Codigo', dataIndex: 'codigo_Ceco', key: 'codigo_Ceco', align: 'center' },

    ]

    if (user.isAdmin) {
        columns.push(

            {
                title: 'Acciones', key: 'edit', align: 'center', responsive: ['md'], render: (text, record) => <Actions Ceco={record} refetch={refetch} />
            }
        )
    }

    return (
        <div>
            <Breadcrumb
                items={[
                    {
                        title: <a href="/solicitud">Inicio</a>,
                    },
                    {
                        title: 'Centros de Costo',
                    },
                ]}
                className='text-sm mb-4'
            />

            <div className='flex flex-col mb-4'>
                <p className='font-semibold text-lg leading-none'>
                    Centro de Costos
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
                dataSource={isSuccess ? (isNotEmpty(search.ceco) ? search.data : data) : []}
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

export default CentroDeCosto