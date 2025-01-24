import React from 'react';

const AuthContext = React.createContext({
    isSignedIn: false,
    user: {}

});

export default AuthContext;