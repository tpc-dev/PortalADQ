import React from 'react'
import Logo from '../../assets/LogoBlanco.png'

const Loading = () => (
    <div className="w-full min-h-screen  bg-app flex justify-center items-center">
        <img src={Logo} alt="Logo" width={300} className="animate-bounce" />
    </div>
)

export default Loading