import { useQuery } from 'react-query'
import Departamento from '../service/Departaments'

const useDepartament = () => {

    const response = useQuery({
        queryKey: ['departaments'],
        queryFn: () => Departamento.get(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useDepartament