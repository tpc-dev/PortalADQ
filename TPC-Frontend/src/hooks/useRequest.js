import { useQuery } from 'react-query'
import Request from '../service/Request'

const useRequest = () => {

    const response = useQuery({
        queryKey: ['request'],
        queryFn: () => Request.get(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useRequest