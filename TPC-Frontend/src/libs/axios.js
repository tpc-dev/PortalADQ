import axios from 'axios';


const createInstace = baseURL => {
    const instance = axios.create({
        headers: {

            'Access-Control-Allow-Origin': '*',
        },
        baseURL
    })


    instance.interceptors.response.use(res => res.data, err => { throw err });


    return instance;
}

export default createInstace;