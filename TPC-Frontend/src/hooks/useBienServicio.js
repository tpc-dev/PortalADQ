import { useQuery } from 'react-query'
import BienServicio from '../service/Bien_Servicios'

const useBienServicio = () => {

    const response = useQuery({
        queryKey: ['BienServicio'],
        queryFn: () => BienServicio.get(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useBienServicio