import { useQuery } from 'react-query'
import Liberadores from '../service/Liberadores'

const useLiberadores = () => {

    const response = useQuery({
        queryKey: ['liberadores'],
        queryFn: () => Liberadores.list(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useLiberadores