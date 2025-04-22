import React, { useState } from 'react'
import Table from '../../Template/Table'
import ProvidersData from '../../../data/Providers.json'
import { Breadcrumb, Button } from 'antd'
import { BsEye } from 'react-icons/bs'
import { FaEye } from 'react-icons/fa'
import { Link, navigate } from 'gatsby'
import Create from './Create'
import Update from './Update'
import useProviders from '../../../hooks/useProviders'
import useAuthContext from '../../../hooks/useAuthContext'
import { normalizeText } from '../../../utils/paragraph'
import Search from './Search'
import { isNotEmpty } from '../../../utils/validations'
import Excel from './Excel'
import { LayoutOutlined } from '@ant-design/icons'; 
import Proveedor from '../../../service/Providers'
import { saveAs } from 'file-saver';

function Providers() {


    const { user } = useAuthContext()

    const { data, isLoadign, isSuccess, refetch } = useProviders()

    const [search, setSearch] = useState({
        data: [],
        provider: ''
    })


    const handleSearch = e => {
        const provider = normalizeText(e.target.value)
        const result = data.filter(p => normalizeText(p.rut_Proveedor).includes(provider) || normalizeText(p.razon_Social).includes(provider) || normalizeText(p.nombre_Fantasia).includes(provider) || normalizeText(p.comuna).includes(provider))
        setSearch({
            data: result,
            provider
        })
    }

    const DescargarEjemplo = async () => {
       
  
        try {
             const response = await Proveedor.ExampleProveedor();
             const data = response.archivoDoc;
             const fileName = response.nombreDoc.trim();
     
             // Verificar que `data` es un array de bytes o una cadena base64
             let byteArray;
             if (typeof data === 'string') {
                 // Si es una cadena base64, decodificarla
                 const binaryString = atob(data);
                 const len = binaryString.length;
                 byteArray = new Uint8Array(len);
                 for (let i = 0; i < len; i++) {
                     byteArray[i] = binaryString.charCodeAt(i);
                 }
             } else {
                 // Si ya es un array de bytes, usarlo directamente
                 byteArray = data;
             }
     
             // Mapear extensiones a tipos MIME
             const extension = fileName.split('.').pop().toLowerCase();
             const mimeTypes = {
                 xlsx: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
             };
             const contentType = mimeTypes[extension] || 'application/octet-stream';
     
             // Crear el Blob y descargar el archivo
             const blob = new Blob([byteArray], { type: contentType });
             saveAs(blob, fileName);
     
         } catch (error) {
             console.error('Error al descargar el archivo:', error);
             alert('Hubo un error al descargar el archivo. Por favor, inténtalo de nuevo más tarde.');
         }
     };
    const columns = [
        { title: 'Id', dataIndex: 'iD_Proveedores', key: 'id', sorter: (a, b) => a.iD_Proveedores - b.iD_Proveedores, align: 'center', showSorterTooltip: { target: 'full-header' }, sortDirections: ['descend'], defaultSortOrder: 'descend' },
        { title: 'Rut', dataIndex: 'rut_Proveedor', key: 'rut', align: 'left', responsive: ['md'] },
        { title: 'Razon social', dataIndex: 'razon_Social', key: 'company_name', align: 'left', responsive: ['md'] },
        { title: 'Nombre fantasia', dataIndex: 'nombre_Fantasia', key: 'fantasy_name', align: 'center' },
        { title: 'Bien/Servicio', dataIndex: 'iD_Bien_Servicio', key: 'bien_servicio', align: 'center' },
        { title: 'Comuna', dataIndex: 'comuna', key: 'commune', align: 'center', responsive: ['md'] },
        { title: 'Contacto', dataIndex: 'telefono_Proveedor', key: 'contact', align: 'center' },
        { title: 'Estado', dataIndex: 'estado', key: 'contact', align: 'center', render: (text, record) => record.estado ? 'Activo' : 'Inactivo' },
        {
            title: 'Detalle', key: 'detail', align: 'left', responsive: ['md'], render: (text, record) => <Button onClick={() => navigate(`/providers/${record.iD_Proveedores}`, { state: record })} >
                <FaEye />
            </Button>
        },
    ]


    if (user.isAdmin) {
        columns.push({
            title: 'Editar', key: 'edit', align: 'left', responsive: ['md'], render: (text, record) => <Update proveedor={record} refetch={refetch} />
        },)
    }

    return (
        <div>
            <Breadcrumb
                items={[
                    {
                        title: <a href="/solicitud">Inicio</a>,
                    },
                    {
                        title: 'Lista de proveedores',
                    },
                ]}
                className='text-sm mb-4'
            />
<div className='flex flex-row items-center mb-4'>
    <p className='font-semibold text-lg leading-none mr-2'>
        
        Proveedores
        
    </p>
    <Button icon={<LayoutOutlined />} onClick={DescargarEjemplo}>
    </Button>
             
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
                loading={isLoadign}
                columns={columns}
                data={isSuccess ? (isNotEmpty(search.provider) ? search.data : data) : []}
            />
        </div>
    )
}

export default Providers