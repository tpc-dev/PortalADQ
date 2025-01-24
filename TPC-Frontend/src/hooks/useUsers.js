import { useQuery } from 'react-query'
import User from '../service/User'

const useUsers = () => {

    const response = useQuery({
        queryKey: ['users'],
        queryFn: () => User.list(),
        refetchOnWindowFocus: false,
        retry: 1
    })

    return response
}

export default useUsers