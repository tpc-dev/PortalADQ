import { Breadcrumb, Table } from 'antd'
import React, { useState } from 'react'
import DepartamentosData from '../../../data/Departamentos.json'
import Create from './Create'
import Update from './Update'
import Actions from './Actions'
import Search from './Search'
import { normalizeText } from '../../../utils/paragraph'
import { isNotEmpty } from '../../../utils/validations'
import useAuthContext from '../../../hooks/useAuthContext'
import useDepartament from '../../../hooks/useDepartament'

function Departaments() {

    const { user } = useAuthContext()

    const { data, isLoading, isSuccess, refetch } = useDepartament()



    const [search, setSearch] = useState({
        data: [],
        departament: ''
    })


    const handleSearch = e => {
        const departament = normalizeText(e.target.value)
        const result = data.filter(u => normalizeText(u.id_Departamento.toString()).includes(departament) || normalizeText(u.nombre).includes(departament) || normalizeText(u.descripcion).includes(departament) || normalizeText(u.encargado).includes(departament))
        setSearch({
            data: result,
            departament
        })
    }

    const columns = [
        { title: 'Id', dataIndex: 'id_Departamento', key: 'id_Departamento', align: 'left', responsive: ['md'] },
        { title: 'Nombre', dataIndex: 'nombre', key: 'nombre', align: 'left', responsive: ['md'] },
        { title: 'Descripcion', dataIndex: 'descripcion', key: 'descripcion', align: 'center' },
        { title: 'Encargado', dataIndex: 'encargado', key: 'encargado', align: 'center' },
    ]

    if (user.isAdmin == true) {
        columns.push({
            title: 'Acciones', key: 'edit', align: 'center', responsive: ['md'], render: (text, record) => <Actions departamento={record} refetch={refetch} />
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
                        title: 'Lista de departamentos',
                    },
                ]}
                className='text-sm mb-4'
            />
            <div className='flex flex-col mb-4'>
                <p className='font-semibold text-lg leading-none'>
                    Departamentos
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
                dataSource={isSuccess ? (isNotEmpty(search.departament) ? search.data : data) : []}
                rowKey='id_Departamento'
            />
        </div>
    )
}

export default Departaments