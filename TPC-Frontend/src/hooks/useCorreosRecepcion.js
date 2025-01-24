import { useQuery } from 'react-query'
import Recepcion from '../service/Recepcion'

const useCorreosRecepcion = () => {

    const response = useQuery({
        queryKey: ['correosRecepcion'],
        queryFn: () => Recepcion.get(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useCorreosRecepcion