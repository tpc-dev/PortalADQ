import { useQuery } from 'react-query'
import Providers from '../service/Providers'

const useProviders = () => {

    const response = useQuery({
        queryKey: ['providers'],
        queryFn: () => Providers.get(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useProviders