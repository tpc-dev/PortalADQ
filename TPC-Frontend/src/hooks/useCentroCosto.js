import { useQuery } from 'react-query'
import CentroCosto from '../service/CentroCosto'

const useCentroCosto = () => {

    const response = useQuery({
        queryKey: ['CentroCosto'],
        queryFn: () => CentroCosto.list(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useCentroCosto