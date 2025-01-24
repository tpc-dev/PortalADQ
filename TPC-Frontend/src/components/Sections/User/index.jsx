import { Breadcrumb, Button } from 'antd'
import React, { useState } from 'react'
import Table from '../../Template/Table'
import UsuariosData from '../../../data/Usuarios.json'
import Search from './Search'
import { normalizeText } from '../../../utils/paragraph'
import { isNotEmpty } from '../../../utils/validations'
import Create from './Create'
import useAuthContext from '../../../hooks/useAuthContext'
import useUsers from '../../../hooks/useUsers'
import Edit from './Edit'
import Excel from './Excel'
import Actions from './Actions'
import { FaEye } from 'react-icons/fa'
import { navigate } from 'gatsby'

function Usuarios() {

    const { user } = useAuthContext()


    const { data, isLoading, isSuccess, refetch } = useUsers()

    const [search, setSearch] = useState({
        data: [],
        user: ''
    })




    const handleSearch = e => {
        const user = normalizeText(e.target.value)
        const result = data.filter(t => normalizeText(t.Nombre_Usuario).includes(user) || normalizeText(t.Apellido_Paterno).includes(user) || normalizeText(t.Apellido_Materno).includes(user) || normalizeText(t.Correo_Usuario).includes(user) || normalizeText(t.Departamento).includes(user) || normalizeText(t.rol).includes(user) || normalizeText(t.Centro_Costo).includes(user))
        setSearch({
            data: result,
            user
        })
    }


    const columns = [
        {
            title: 'ID', dataIndex: 'id_Usuario', key: 'id_Usuario', align: 'left', responsive: ['md'],
        },
        {
            title: 'Nombre', dataIndex: 'nombre_Usuario', key: 'name', align: 'left', responsive: ['md'],
        },
        { title: 'Correo', dataIndex: 'correo_Usuario', key: 'correo_Usuario', align: 'left', responsive: ['md'] },
        // { title: 'Departamento', dataIndex: 'Departamento', key: 'Departamento', align: 'center', responsive: ['md'] },
        {
            title: 'Rol', dataIndex: 'rol', key: 'rol', align: 'center', responsive: ['md'], render: (text, record) => {
                return (
                    <div>
                        {record.admin == true ? 'Administrador' : 'Usuario'}
                    </div>
                )
            }
        },
        {
            title: 'Detalle', key: 'detalle', align: 'center', responsive: ['md'], render: (text, record) => {
                return <Button onClick={() => navigate(`/users/${record.id_Usuario}`, {
                    state: record
                })}>
                    <FaEye />
                </Button>
            }
        },
        {
            title: 'Editar', key: 'edit', align: 'center', responsive: ['md'], render: (text, record) => {
                return <Actions user={record} refetch={refetch} />
            }
        }

        // { title: 'Centro de costos', dataIndex: 'Centro_Costo', key: 'Centro_Costo', align: 'center', responsive: ['md'] },
    ]




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
                        title: 'Gestion de Usuarios',
                    }
                ]}
                className='text-sm mb-4'
            />
            <div className='flex justify-between flex-col lg:flex-row items-center mb-4'>
                <p className='font-semibold text-lg leading-none'>
                    Usuarios
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
                            <Excel />
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

export default Usuarios