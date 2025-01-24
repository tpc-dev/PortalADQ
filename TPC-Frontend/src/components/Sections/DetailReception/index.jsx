import { Breadcrumb, Button } from 'antd'
import React, { useState } from 'react'
import RecepcionOcData from '../../../data/RecepcionOc.json'
import Table from '../../Template/Table'
import { isNotEmpty } from '../../../utils/validations'
import { normalizeText } from '../../../utils/paragraph'
import Search from './Search'
import { navigate } from 'gatsby'
import { FaEye } from 'react-icons/fa'
import Cart from './Cart'
import useRequestOC from '../../../hooks/useRequestOC'

function DetalleRecepcion() {


    const { data, isLoading, isSuccess, isError, refetch } = useRequestOC()

    const [search, setSearch] = useState({
        data: [],
        ticket: ''
    })

    const handleSearch = (e) => {
        const ticket = normalizeText(e.target.value)
        const result = RecepcionOcData.filter(t => normalizeText(t.ticket).includes(ticket) || normalizeText(t.oc).includes(ticket) || normalizeText(t.fechaCreacion).includes(ticket) || normalizeText(t.proveedor).includes(ticket) || normalizeText(t.ceco).includes(ticket))
        setSearch({
            data: result,
            ticket
        })
    }

    const columns = [

        { title: 'Ticket', dataIndex: 'iD_Ticket', key: 'ticket', align: 'center' },
        { title: 'OC', dataIndex: 'numero_OC', key: 'oc', align: 'center' },
        { title: 'Fecha Creación', dataIndex: 'fecha_Creacion_OC', key: 'fecha_creacion', align: 'center' },
        {
            title: 'Ver', key: 'detail', align: 'center', responsive: ['md'], render: (text, record) =>
                <div className='flex justify-center gap-2'>
                    <Button onClick={() => navigate(`/requests-oc/reception`, {
                        state: { ticket: record }
                    })} className='px-2'>
                        <FaEye />
                    </Button>
                    <Cart record={record} />
                </div>
        },

    ]

    return (
        <div>
            <Breadcrumb
                items={[
                    {
                        title: <a href="/solicitud">Inicio</a>,
                    },
                    {
                        title: 'Contabilidad',
                    },
                    {
                        title: 'Detalle Recepción',
                    }
                ]}

                className='text-sm mb-4'
            />

            <div className='flex justify-between flex-col lg:flex-row items-center mb-4'>
                <p className='font-semibold text-lg leading-none'>
                    Detalle Recepcion Oc
                </p>
            </div>
            <div className="flex flex-col lg:flex-row items-end justify-end gap-2 mb-4">
                <div className="flex-1 order-1">
                </div>
                <div className="flex gap-x-2 order-2">
                    {/* <Create /> */}
                </div>
                <Search onChange={handleSearch} />

            </div>

            <Table
                columns={columns}
                data={isSuccess ? (isNotEmpty(search.ticket) ? search.data : data) : []}
            />


        </div>
    )
}

export default DetalleRecepcion