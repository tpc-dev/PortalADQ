import React from 'react'
import Cancel from './Cancel'
import Update from './Update'

function Actions({ Solicitud, refetch }) {

    return (
        <div className='flex items-center justify-center gap-2'>

            {
                Solicitud.estado !== 'Cancelado' &&
                <>
                    <Cancel solicitud={Solicitud} />
                    <Update solicitud={Solicitud} refetch={refetch} />
                </>
            }
        </div>
    )
}

export default Actions