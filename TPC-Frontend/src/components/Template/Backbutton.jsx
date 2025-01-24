import { FloatButton } from 'antd'
import { navigate } from 'gatsby'
import React from 'react'

import { PiArrowArcLeft } from 'react-icons/pi'
function BackButton() {
    return (
        <div>
            <FloatButton
                type="primary"
                onClick={() => navigate(-1)}
                icon={<PiArrowArcLeft />}
                tooltip={<div>Volver atras</div>}
                className='fixed top-5 left-5'
            />
        </div>
    )
}

export default BackButton