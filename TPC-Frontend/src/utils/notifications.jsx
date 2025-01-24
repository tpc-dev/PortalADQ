import React from 'react';
import { Button, notification, Space } from 'antd';

const App = () => {
    const [api, contextHolder] = notification.useNotification();

    const openNotification = () => {
        const key = `open${Date.now()}`;
        const btn = (
            <Space>
                <Button type="link" size="small" onClick={() => api.destroy()}>
                    Cerrar
                </Button>

            </Space>
        );
        api.open({
            message: 'OC Pendiente',
            description:
                'Tienes Ordenes de compras pendientes por liberar',
            btn,
            key,
            onClose: close,
        });
    };
    return (
        <>
            {contextHolder}
            <Button type="primary" onClick={openNotification}>
                Open the notification box
            </Button>
        </>
    );
};
export default App;