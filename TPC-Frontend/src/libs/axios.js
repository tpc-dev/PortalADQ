import axios from 'axios';


const createInstace = baseURL => {
    const instance = axios.create({
        headers: {
            'Content-type': 'application/json',
            'Access-Control-Allow-Origin': '*',
        },
        baseURL
    })


    //TOKEN DESDE BACKEND
    // instance.interceptors.request.use(async config => {

    //     const { tokens } = await fetchAuthSession({ forceRefresh: true })
    //     const { idToken } = tokens

    //     config.headers.Authorization = `Bearer ${ idToken.toString() }`
    //     return config
    // })

    instance.interceptors.response.use(res => res.data, err => { throw err });


    return instance;
}

export default createInstace;