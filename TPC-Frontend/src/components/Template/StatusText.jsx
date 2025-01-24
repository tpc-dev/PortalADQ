import React, { useEffect } from 'react'
import Status from '../../data/Status.json'
import { Tooltip } from 'antd'

function StatusText({ status }) {



    const [description, setDescription] = React.useState('')


    const searchDescription = (status) => {
        const result = Status.find(element => element.name === status)
     
        setDescription(result?.description)
    }

    useEffect(() => {
        searchDescription(status)
    }, [status])


    return (
        <Tooltip title={description} color='#1135A6'>
            <span>
                {status}
            </span>
        </Tooltip>
    )
}

export default StatusText