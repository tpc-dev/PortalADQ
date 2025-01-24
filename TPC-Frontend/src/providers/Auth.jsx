import { useCallback, useEffect, useState } from "react";
import AuthContext from "../context/Auth";
import React from "react";
import SignIn from "../components/Auth/SignIn";
import Dashboard from "../pages/dashboard";
import User from "../service/User";
import LoadingComponent from "../components/Template/Loading";


const AuthProvider = ({ children }) => {

    const [isSignedIn, setIsSignedIn] = useState(false);
    const [loading, setLoading] = useState(false);
    const [user, setUser] = useState({});


    const setUserAttributes = attributes => {
        const { id_Usuario, correo_Usuario, nombre_Usuario, admin, Tipo_Liberador } = attributes;
        setUser({
            id_Usuario,
            correo_Usuario,
            nombre_Usuario,
            isAdmin: admin ? true : false,
            isLiberador: Tipo_Liberador !== 'No' ? true : false
        });
    }


    const authIsSuccess = useCallback(attributes => {
        setUserAttributes(attributes);
        setIsSignedIn(true);
    }, [])


    useEffect(() => {
        setLoading(true);
        const correo = localStorage.getItem('correo');
        const password = localStorage.getItem('password');
        const user = localStorage.getItem('user');
        const fecha = localStorage.getItem('fecha');

        //expiracion de 12 horas
        if (fecha && new Date().getTime() - new Date(fecha).getTime() > 43200000) {
            localStorage.clear();
            setIsSignedIn(false);
        }

        if (user) {
            setUserAttributes(JSON.parse(user));
            setIsSignedIn(true);
        }

        setTimeout(() => {
            setLoading(false);
        }, 2000);

    }, [authIsSuccess])



    return (
        <AuthContext.Provider value={{ isSignedIn, setIsSignedIn, user, authIsSuccess }}>
            {
                loading ? <LoadingComponent /> : <>
                    {children}
                </>
            }
        </AuthContext.Provider>
    )
}

export default AuthProvider;