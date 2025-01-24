import React from 'react'
import Edit from './Edit'
import Delete from './Delete'
import useAuthContext from '../../../hooks/useAuthContext'


function Actions({ user, refetch }) {

    const dataUser = useAuthContext()


    console.log(dataUser.user.isAdmin)

    return (
        <div className='flex items-center justify-center gap-2'>
            {
                dataUser.user.id_Usuario == user.id_Usuario && dataUser.user.isAdmin == false &&
                <Edit user={user} refetch={refetch} />
            }
            {
                dataUser.user.isAdmin == true &&
                <Edit user={user} refetch={refetch} />
            }
            {
                dataUser.user.isAdmin == true &&
                <Delete user={user} refetch={refetch} />

            }
        </div>
    )
}

export default Actions