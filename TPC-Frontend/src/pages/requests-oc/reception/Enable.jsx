import React, { useState } from 'react'
import { Switch } from 'antd'
import { alertError } from '../../../utils/alert'
import RequestOC from '../../../service/RequestOc'

const Enable = ({ OC }) => {

    const [loading, setLoading] = useState(false)
    const [checked, setChecked] = useState(OC?.recepcion)

    const handleEnableOC = () => {

        setLoading(true)
        setChecked(!checked)

        RequestOC.Enable(OC.id_Orden_Compra, {
            ...OC,
            recepcion: !checked
        })
            .catch(() => {
                alertError()
                setChecked(checked)
            })
            .finally(() => setLoading(false))
    }

    return (
        <Switch
            size="small"
            checked={checked}
            onChange={() => handleEnableOC()}
            loading={loading}
        />
    )
}

export default Enable