import { Breadcrumb } from 'antd'
import Table from '../../Template/Table'
import React, { useState } from 'react'
import HistorialCorreoData from '../../../data/HistorialCorreo.json'
import Search from './Search'
import { normalizeText } from '../../../utils/paragraph'
import { isNotEmpty } from '../../../utils/validations'
import useCorreosRecepcion from '../../../hooks/useCorreosRecepcion'



function HistorialEmail() {

    // N° Ticket / N° OC / Usuario / Fecha Envio / Fecha Respuesta / Respuesta / Proveedor / CeCo / Comentarios / Acciones (el ojo pa ver)

    const { data, isLoading, isSuccess, isError } = useCorreosRecepcion()

    const [search, setSearch] = useState({
        data: [],
        email: ''
    })

    console.log(data)

    const columns = [
        { title: 'N° Ticket', dataIndex: 'n_Ticket', key: 'ticket', align: 'center' },
        { title: 'N° OC', dataIndex: 'n_OC', key: 'oc', align: 'center' },
        { title: 'Usuario', dataIndex: 'usuario', key: 'usuario', align: 'center' },
        { title: 'Fecha Envio', dataIndex: 'fechaEnvio', key: 'fecha_envio', align: 'center' },
        { title: 'Proveedor', dataIndex: 'proveedor', key: 'proveedor', align: 'center' },
        { title: 'CeCo', dataIndex: 'ceco', key: 'ceco', align: 'center' },
    ]

    const handleSearch = (e) => {
        const email = normalizeText(e.target.value)
        const result = HistorialCorreoData.filter(t => normalizeText(t.ticket).includes(email) || normalizeText(t.oc).includes(email) || normalizeText(t.usuario).includes(email) || normalizeText(t.fechaEnvio).includes(email) || normalizeText(t.fechaRespuesta).includes(email) || normalizeText(t.respuesta).includes(email) || normalizeText(t.proveedor).includes(email) || normalizeText(t.ceco).includes(email) || normalizeText(t.comentarios).includes(email))
        setSearch({
            data: result,
            email
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
                        title: 'Contabilidad',
                    },
                    {
                        title: 'Envio Correo Recepcion',
                    }
                ]}

                className='text-sm mb-4'
            />
            <div className='flex justify-between flex-col lg:flex-row items-center mb-4'>
                <p className='font-semibold text-lg leading-none'>
                    Envio Correo Recepcion
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
                data={isSuccess ? (isNotEmpty(search.email) ? search.data : data) : []}
            />

        </div>
    )
}

export default HistorialEmail