import { useQuery } from 'react-query'
import Reemplazos from '../service/Replacements'

const useCentroCosto = () => {

    const response = useQuery({
        queryKey: ['Remplazos'],
        queryFn: () => Reemplazos.list(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useCentroCosto