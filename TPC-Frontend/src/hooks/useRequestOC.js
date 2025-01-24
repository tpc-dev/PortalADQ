import { useQuery } from 'react-query'
import RequestOc from '../service/RequestOc'

const useRequestOC = () => {

    const response = useQuery({
        queryKey: ['requestOC'],
        queryFn: () => RequestOc.get(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useRequestOC