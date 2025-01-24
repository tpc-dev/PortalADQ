import React from 'react'
import { Input } from 'antd'
import { SearchOutlined } from '@ant-design/icons'

const Search = ({ onChange }) => {

    return (
        <Input
            placeholder="Buscar Reemplazo"
            allowClear={{ clearIcon: <span>Limpiar</span> }}
            prefix={<SearchOutlined />}
            onChange={onChange}
            variant="filled"
            className="w-80"
        />
    )
}

export default Search