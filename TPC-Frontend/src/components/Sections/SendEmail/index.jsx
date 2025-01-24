import { Breadcrumb, Button } from 'antd'
import React, { useState } from 'react'
import Table from '../../Template/Table'
import { BiCheck, BiX } from 'react-icons/bi';
import Email from './email';
import Search from './Search';
import { normalizeText } from '../../../utils/paragraph';
import { isNotEmpty } from '../../../utils/validations';
import useCorreoRecepcion from '../../../hooks/useCorreoRecepcion';
import { navigate } from 'gatsby';
import { HiOutlineDocumentText } from 'react-icons/hi';
import { useCorreosRecepcion } from '../../../hooks/useCorreosRecepcion';

function EnviarCorreo() {
    // N° Ticket / N° OC / Solicitante / Proveedor / CeCo / Fecha Creacion / Correos Enviados / Primer Correo / Ultimo Correo / Recepcion


    const { data, isSuccess, refetch } = useCorreoRecepcion()
    const [selectedRowKeys, setSelectedRowKeys] = useState([]);

    const [search, setSearch] = useState({
        data: [],
        email: ''
    })

    const handleSearch = e => {
        const email = normalizeText(e.target.value)
        const result = data.filter(u => normalizeText(u.id_Ticket.toString()).includes(email) || normalizeText(u.numero_OC.toString()).includes(email) || normalizeText(u.solicitante).includes(email) || normalizeText(u.proveedor).includes(email) || normalizeText(u.ceCo).includes(email) || normalizeText(u.fechaCreacion).includes(email) || normalizeText(u.correosEnviados).includes(email) || normalizeText(u.primerCorreo).includes(email) || normalizeText(u.ultimoCorreo).includes(email) || normalizeText(u.detalle).includes(email))
        setSearch({
            data: result,
            email
        })
    }


    const columns = [
        { title: 'N° Ticket', dataIndex: 'id_Ticket', key: 'ticket', align: 'center' },
        { title: 'N° OC', dataIndex: 'numero_OC', key: 'oc', align: 'center' },
        { title: 'Solicitante', dataIndex: 'solicitante', key: 'solicitante', align: 'center' },
        { title: 'Proveedor', dataIndex: 'proveedor', key: 'proveedor', align: 'center' },
        { title: 'CeCo', dataIndex: 'ceCo', key: 'ceCo', align: 'center' },
        {
            title: 'Fecha Creacion', dataIndex: 'fechaCreacion', key: 'fecha_creacion', align: 'center', render: (text) => <span>{
                new Date(text).toLocaleDateString()
            } </span>
        },
        { title: 'Correos Enviados', dataIndex: 'correosEnviados', key: 'correos_enviados', align: 'center' },
        {
            title: 'Primer Correo', dataIndex: 'primerCorreo', key: 'primer_correo', align: 'center', render: (text) => <span>{
                new Date(text).toLocaleDateString()
            }</span>
        },
        {
            title: 'Ultimo Correo', dataIndex: 'ultimoCorreo', key: 'ultimo_correo', align: 'center', render: (text) => <span>{
                new Date(text).toLocaleDateString()
            } </span>
        },
        { title: 'Detalle', dataIndex: 'detalle', key: 'recepcion', align: 'center' },
        {
            title: 'Historial',
            key: 'historial',
            align: 'center',
            render: (text, record) => {
                return (
                    <div className='flex justify-center gap-2'>
                        <Button onClick={() => navigate(`/requests-oc/reception/`, {
                            state: { ticket: record }
                        })}>
                            <HiOutlineDocumentText />
                        </Button>
                    </div>
                )
            }

        },
        {
            title: 'Seleccionar',
            key: 'selection',
            align: 'center',
            render: (text, record) => {
                return (
                    <div className='flex justify-center gap-2'>
                        {

<Button
className='px-2'
onClick={() => {
    const selected = selectedRowKeys.includes(record.id_Ticket)
    if (selected) {
        setSelectedRowKeys(selectedRowKeys.filter(key => key !== record.id_Ticket))
    } else {
        setSelectedRowKeys([...selectedRowKeys, record.id_Ticket])
    }
}}
>
{selectedRowKeys.includes(record.id_Ticket) ? <BiCheck /> : <BiX />}
</Button>
                        }
                    </div>
                )
            }
        }
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
                        title: 'Envio de Correos',
                    }
                ]}

                className='text-sm mb-4'
            />
            <div className='flex justify-between flex-col lg:flex-row items-center mb-4'>
                <p className='font-semibold text-lg leading-none'>
                    Envio de Correos
                </p>
            </div>
            <div className="flex flex-col lg:flex-row items-end justify-end gap-2 mb-4">
                <div className="flex-1 order-1">
                </div>
                <div className="flex gap-x-2 order-2">
                    <Email selectedRowKeys={selectedRowKeys} allId={data} refetch={refetch} />
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

export default EnviarCorreo