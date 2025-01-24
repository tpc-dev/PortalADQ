import React from 'react'
import Update from './Update'
import Delete from './Delete'

function Actions({ Ceco, refetch }) {
    return (
        <div className='flex items-center justify-center gap-2'>
            <Update Ceco={Ceco} refetch={refetch} />
            <Delete Ceco={Ceco} refetch={refetch} />
        </div>
    )
}

export default Actions