import { Link } from 'gatsby'
import React from 'react'
import { PiCheckBold } from 'react-icons/pi'

function Confirmacion({ data }) {
    return (
        <div className='py-20 flex flex-col gap-5 items-center justify-center '>
            <PiCheckBold className='text-app text-9xl animate-bounce' />
            <p className='text-2xl '>
                {
                    data.iD_Ticket ? `Orden de compra generada con exito` : `Cotizacion generada con exito`
                }
            </p>
            <Link to={data.iD_Ticket ? '/requests-oc' : '/requests'} className='bg-app text-white px-4 py-2 rounded-md text-xl hover:text-app hover:bg-white transition-all duration-200'>Volver al inicio</Link>

        </div>
    )
}

export default Confirmacion