import { Breadcrumb, Table } from 'antd'
import React, { useState } from 'react'
import Actions from './Actions'
import Create from './Create'
import Search from './Search'
import { normalizeText } from '../../../utils/paragraph'
import { isNotEmpty } from '../../../utils/validations'
import useAuthContext from '../../../hooks/useAuthContext'
import useOrdenesEstadisticas from '../../../hooks/useOrdenesEstadisticas'
import Excel from './Excel'
import { EyeOutlined, LayoutOutlined } from '@ant-design/icons'; 
import { Button } from 'antd';
import RequestOC2 from '../../../service/RequestOc'
import { saveAs } from 'file-saver';

function OrderStatistics() {


    const { user } = useAuthContext()

    const { data, isLoading, isSuccess, isError, error, refetch } = useOrdenesEstadisticas()

    const [search, setSearch] = useState({
        data: [],
        order: ''
    })

    const handleSearch = e => {
        const order = normalizeText(e.target.value)
        const result = data.filter(u => normalizeText(u.id_Orden_Estadistica.toString()).includes(order) || normalizeText(u.nombre).includes(order) || normalizeText(u.id_Centro_de_Costo).includes(order) || normalizeText(u.codigo_OE).includes(order))

        setSearch({
            data: result,
            order
        })
    }
    const DescargarEjemplo = async () => {
       
  
        try {
             const response = await RequestOC2.ExampleOC();
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
        { title: 'Id', dataIndex: 'id_Orden_Estadistica', key: 'id_Orden_Estadistica', align: 'left', responsive: ['md'], defaultSortOrder: 'ascend', sorter: (a, b) => a.id_Orden_Estadistica - b.id_Orden_Estadistica },
        { title: 'Nombre', dataIndex: 'nombre', key: 'nombre', align: 'left', responsive: ['md'] },
        { title: 'Centro de costo', dataIndex: 'id_Centro_de_Costo', key: 'id_Centro_de_Costo', align: 'center' },
        { title: 'Orden Estadistica', dataIndex: 'codigo_OE', key: 'codigo_OE', align: 'center' },

    ]

    if (user.isAdmin) {
        columns.push(
            {
                title: 'Acciones', key: 'edit', align: 'center', responsive: ['md'], render: (text, record) => <Actions order={record} refetch={refetch} />
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
                        title: 'Ordenes Estadisticas',
                    },
                ]}
                className='text-sm mb-4'
            />

<div className='flex flex-row items-center mb-4'>
    <p className='font-semibold text-lg leading-none mr-2'>
        
        Ordenes Estadisticas
        
    </p>
    <Button icon={<LayoutOutlined />} onClick={DescargarEjemplo}>
            </Button>
             
              
 
            </div>
            <div className="flex flex-col lg:flex-row items-center gap-2 mb-4">
                <div className="flex-1 order-1">
                    <p className="text-sm text-[#556a89]">Listado de Ordenes Estadisticas</p>
           

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
                dataSource={isSuccess ? (isNotEmpty(search.order) ? search.data : data.sort((a, b) => a.id_Orden_Estadistica - b.id_Orden_Estadistica)) : []}
                rowKey='id_Orden_Estadistica'
                pagination={{ pageSize: 10, showSizeChanger: false }}
                showSorterTooltip={true}
                sortDirections={['ascend', 'descend']}
                key={'id_Orden_Estadistica'}

            />

        </div>
    )
}

export default OrderStatistics