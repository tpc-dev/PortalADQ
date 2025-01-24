import { useQuery } from 'react-query'
import OrdenesEstadisticas from '../service/OrdenEstadistica'

const useOrdenesEstadisticas = () => {

    const response = useQuery({
        queryKey: ['request'],
        queryFn: () => OrdenesEstadisticas.list(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useOrdenesEstadisticas