
export const setLocalStorage = (key, value) => window.localStorage.setItem(key, value)

export const setSessionStorage = (key, value) => window.sessionStorage.setItem(key, value)

export const getLocalStorage = key => typeof window !== 'undefined' ? window.localStorage.getItem(key) : null

export const getSessionStorage = key => typeof window !== 'undefined' ? window.sessionStorage.getItem(key) : null

export const removeLocalStorage = key => window.localStorage.removeItem(key)

export const removeSessionStorage = key => window.sessionStorage.removeItem(key)

export const removeAllLocalStorage = () => window.localStorage.clear()

export const removeAllSessionStorage = () => window.sessionStorage.clear()