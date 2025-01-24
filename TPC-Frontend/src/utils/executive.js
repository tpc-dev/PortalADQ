export const getGoalVisitPercent = (goal, visits) => {


    //Mover a Backend
    const startDate = new Date(goal.start)
    const endDate = new Date(goal.expiration)

    var leftDays = Math.ceil((endDate - startDate) / (1000 * 60 * 60 * 24)) + ' días restantes'

    if (leftDays < 0) leftDays = 'Fecha de expiración vencida'

    const visitsInRange = visits?.filter(visit => {
        const visitDate = new Date(visit.createdAt)
        return visitDate >= startDate && visitDate <= endDate && visit.state_id === 'accepted'
    })


    return {
        percent: Math.round((visitsInRange.length / goal.visit) * 100),
        visitsInRange: visitsInRange.length,
        leftDays
    }



}