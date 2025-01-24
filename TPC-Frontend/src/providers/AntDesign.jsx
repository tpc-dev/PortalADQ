import React from 'react'
import { ConfigProvider } from 'antd'
import esES from 'antd/lib/locale/es_ES'
import Theme from '../data/Theme.json'

const AntdProvider = ({ children }) => (
    <ConfigProvider locale={esES} theme={Theme}>
        {children}
    </ConfigProvider>
)

export default AntdProvider