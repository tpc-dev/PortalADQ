import React, { useEffect, useState } from 'react'
import { DatePicker, Input } from 'antd'
import { SearchOutlined } from '@ant-design/icons'

const Search = ({ onChange, handleSearchBetweenDates }) => {


    const [date1, setDate1] = useState(null)
    const [date2, setDate2] = useState(null)


    const handleDate1 = (date, dateString) => {
        setDate1(dateString)
    }

    const handleDate2 = (date, dateString) => {
        setDate2(dateString)
    }

    useEffect(() => {
        if (date1 && date2) {
            handleSearchBetweenDates(date1, date2)
        } else {
            handleSearchBetweenDates(null, null)
        }
    }, [date1, date2])




    return (
        <div className='flex gap-5'>
            <Input
                placeholder="Buscar Solicitud"
                allowClear={{ clearIcon: <span>Limpiar</span> }}
                prefix={<SearchOutlined />}
                onChange={onChange}
                variant="filled"
                className="w-80"
            />

            <DatePicker
                onChange={(date, dateString) => handleDate1(date, dateString)}
                allowClear={{ clearIcon: <span>Limpiar</span> }}
                prefix={<SearchOutlined />}
                variant='filled'
                className='w-80'
            />

            <DatePicker
                onChange={(date, dateString) => handleDate2(date, dateString)}
                allowClear={{ clearIcon: <span>Limpiar</span> }}
                prefix={<SearchOutlined />}
                variant='filled'
                className='w-80'
            />


        </div>
    )
}

export default Search