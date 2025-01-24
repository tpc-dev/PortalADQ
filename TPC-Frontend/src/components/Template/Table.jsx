import React from 'react'
import { Table as AntdTable } from 'antd'
import { useResizeDetector } from 'react-resize-detector'

const Table = ({ columns, loading, data, ActiverowSelection, selectedRows, setSelectedRowKeys }) => {

    const { ref, height } = useResizeDetector()

    return (
        <div ref={ref} className="h-[calc(100vh-142px)] md:h-[calc(100vh-220px)] lg:h-[calc(100vh-180px)]">
            <AntdTable
                size="small"
                rowKey="ticket"
                columns={columns}
                dataSource={data}
                pagination={{
                    position: ['bottomCenter'],
                    pageSize: 15,
                    size: 'default',
                    showSizeChanger: false,
                }}
                locale={{ emptyText: false ? 'Cargando' : 'No se han encontrado registros' }}
                scroll={{ y: isNaN(height) ? 200 : height - 90 }}
            />
        </div>
    )
}

export default Table