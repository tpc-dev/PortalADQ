import React from 'react'
import Update from './Update'
import Delete from './Delete'

function Actions({ order, refetch }) {
    return (
        <div className='flex items-center justify-center gap-2'>
            <Update order={order} refetch={refetch} />
            <Delete order={order} refetch={refetch} />
        </div>
    )
}

export default Actions