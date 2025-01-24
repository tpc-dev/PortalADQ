import { useQuery } from 'react-query'
import CorreoRecepcion from '../service/CorreoRecepcion'

const useCorreoRecepcion = () => {

    const response = useQuery({
        queryKey: ['correoRecepcion'],
        queryFn: () => CorreoRecepcion.list(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useCorreoRecepcion