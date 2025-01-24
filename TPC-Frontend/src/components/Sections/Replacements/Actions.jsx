import React from 'react'
import Cancel from './Cancel'
import Update from './Update'

function Actions({ Reemplazo, refetch }) {
    return (
        <div className='flex items-center justify-center gap-2'>

            <Update reemplazo={Reemplazo} refetch={refetch} />
            <Cancel reemplazo={Reemplazo} refetch={refetch} />
        </div>
    )
}

export default Actions