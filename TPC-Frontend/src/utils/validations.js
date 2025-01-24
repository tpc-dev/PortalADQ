export const isUndefined = value => typeof value === 'undefined'

export const isNotUndefined = value => typeof value !== 'undefined'

export const isEmpty = value => value === ''

export const isNotEmpty = value => value !== ''

export const isNull = value => value === null

export const isNotNull = value => value !== null

export const arrayIsEmpty = array => array.length === 0

export const arrayIsNotEmpty = array => array.length > 0

export const arrayMoreThanOneItem = array => array.length > 1

export const objectIsEmpty = object => Object.keys(object).length === 0

export const objectIsNotEmpty = object => Object.keys(object).length > 0

export const objectMoreThanOneItem = object => Object.keys(object).length > 1

export const valueExistInObject = ({ object, key, value }) => object.some(data => data[key] === value)

export const valueExistInArrayOfObjects = ({ array, key, value }) => array.some(obj => obj[key] === value)

export const numberIsOdd = number => number % 2