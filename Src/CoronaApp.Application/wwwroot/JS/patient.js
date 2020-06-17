export default class patient {
    static locations = [];
    static patientLocations = [];
    static locationsForPatient(patientId) {
        patientLocations.splice(0, patientLocations.length);
        patientLocations.push(...(locations.filter(function (location) {
            return location.patientId === patientId;
        })));
        return patientLocations;
    }
}
   