import { useContext } from 'react'
import AuthContext from '../context/Auth'

const useAuthContext = () => {

    const auth = useContext(AuthContext)
    return auth
}

export default useAuthContext