
export const textCapitalize = text => text.split(' ').map(t => t.substring(0, 1).toUpperCase() + t.substring(1).toLowerCase()).join(' ')

export const arrayToParagraph = array => array.join(', ').replace(/, ([^,]*)$/, ' y $1')

// Función que elimina todos los caracteres especiales de una frase o texto
export const normalizeText = text => {
    return text.normalize('NFD').replace(/[\u0300-\u036f]/g, '').replace(/[^\w\s]/gi, '').replace(/  +/g, ' ').trim().toLowerCase()
}
