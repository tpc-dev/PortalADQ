import Modal from 'antd/lib/modal'

export const alertSuccess = ({ title = 'Éxito', message, btnText = 'Continuar', btnFunction = false, width = 500 } = {}) => {
    Modal.destroyAll()
    Modal.success({
        title,
        content: message,
        okText: btnText,
        onOk: btnFunction,
        centered: true,
        zIndex: 3000,
        width,
        okButtonProps: {
            type: 'default',
            className: 'px-4'
        }
    })
}

export const alertWarning = ({ title = 'Atención', message, btnText = 'Lo entiendo', btnFunction = false, width = 400 } = {}) => {
    Modal.destroyAll()
    Modal.warning({
        title,
        content: message,
        okText: btnText,
        onOk: btnFunction,
        centered: true,
        zIndex: 3000,
        width,
        okButtonProps: {
            type: 'default',
            className: 'px-4'
        }
    })
}

export const alertError = ({ title = 'Error', message = `Ha ocurrido un error`, showContact = false, btnText = 'Reintentar', btnFunction = false, width = 500 } = {}) => {
    Modal.destroyAll()
    Modal.error({
        title,
        content: `${ message }${ showContact ? `. Si el problema persiste, comuníquese con cl_operaciones@greatplacetowork.cl` : '' }`,
        okText: btnText,
        onOk: () => btnFunction,
        centered: true,
        zIndex: 3000,
        width,
        okButtonProps: {
            type: 'default',
            className: 'px-4'
        }
    })
}
