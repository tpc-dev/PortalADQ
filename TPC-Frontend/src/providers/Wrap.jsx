import React from 'react'
import AntdProvider from './AntDesign'
import AuthProvider from './Auth'
import QueryProvider from './ReactQuery'

export const wrapPageElement = ({ element }) => (
    <AuthProvider>{element}</AuthProvider>
)

export const wrapRootElement = ({ element }) => (
    <AntdProvider>
        <QueryProvider>
            {element}
        </QueryProvider>
    </AntdProvider>
)

/* 
    wrapRootElement es una función de GatsbyJs que modifica 
    los elementos que envuelven el elemento raíz de la aplicació.
    Ésta función se debe llamar en los archivos gatsby-browser.js y gatsby-ssr.js
*/