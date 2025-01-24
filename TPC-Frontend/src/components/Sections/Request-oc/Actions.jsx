import React from 'react'
import Cancel from './Cancel'
import Update from './Update'

function Actions({ Solicitud, refetch }) {
    return (
        <div className='flex items-center justify-center gap-2'>
            {
                Solicitud.estado !== 'OC Cancelada' &&
                <Cancel solicitud={Solicitud} refetch={refetch} />
            }
            {
                Solicitud.estado !== 'OC Cancelada' &&
                <Update solicitud={Solicitud} refetch={refetch} />
            }
        </div>
    )
}

export default Actions