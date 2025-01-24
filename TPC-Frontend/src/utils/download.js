import axios from 'axios'

// Función que permite la descarga desde un enlace de AWS S3 u otro repositorio.
// responseType: arraybuffer = zip || blob = pdf, xlsx, otro
export const downloadFile = async ({ name, responseType, url }) => {

    return await axios({
        url,
        method: 'GET',
        responseType
    })
    .then(response => {
        let url = window.URL.createObjectURL(new Blob([response.data]))
        let link = document.createElement('a')
            link.href = url
            link.setAttribute('download', name)
            document.body.appendChild(link)
            link.click()
        return true
    })
    .catch(error => {
        throw error
    })
}
