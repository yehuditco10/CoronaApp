let locations = [
    //{
    //    startDate: '2020.05.05 12:00:22',
    //    endDate: '2020.06.05 12:00:22',
    //    city: 'Bney Brak',
    //    location: 'school',
    //    patientId: '111'
    //}, {
    //    startDate: '2020.05.05 12:00:22',
    //    endDate: '2020.06.05 12:00:22',
    //    city: 'Jerusalem',
    //    location: 'library',
    //    patientId: '111'
    //}, {
    //    startDate: '2020.05.05 12:00:22',
    //    endDate: '2020.06.05 12:00:22',
    //    city: 'Elad',
    //    location: 'park',
    //    patientId: '111'
    //},
    //{
    //    startDate: '2020.05.05 12:00:22',
    //    endDate: '2020.06.05 12:00:22',
    //    city: 'Jerusalem',
    //    location: 'school',
    //    patientId: '222'
    //},
    //{
    //    startDate: '2020.05.05 12:00:22',
    //    endDate: '2020.06.05 12:00:22',
    //    city: 'Tel Aviv',
    //    location: 'school',
    //    patientId: '333'
    //}
];

const BASICURL = "https://localhost:44381/api/";
getListFromServer();
let added = false;
const searchBottun = document.getElementById('search');
searchBottun.addEventListener("click", getLocationByPatientId);
document.getElementById("searchAge").addEventListener("click", getLocationByAge);

const patientLocations = [];
const helloTitle = document.createElement('h1');
helloTitle.innerText = 'Epidemiology Report';

document.getElementById("title").appendChild(helloTitle);

function locationsForPatient() {
    cleanTable();
    patientId = document.getElementById('patientID').value;
    patientLocations.splice(0, patientLocations.length);
    patientLocations.push(...(locations.filter(function (location) {
        return location.patientId === patientId;
    })));
    if (patientLocations.length > 0)
        createPathTable(patientLocations);
    if (added === false)
        addAddingOption();
}
function createPathTable(patientLocations,allowEdit) {
    const table = document.getElementById("pathsTable");
    const row = table.insertRow(0);
    const colNames = Object.getOwnPropertyNames(patientLocations[0]);
    const numCol = allowEdit === true ? 5 : 4;
    for (let i = 0; i < numCol; i++) {
        const cell = row.insertCell(i);
        if (i !== 4) {
            cell.innerHTML = colNames[i];
        }
        cell.setAttribute("class", "title");
    }
    for (let i = 0; i < patientLocations.length; i++) {
        const row = table.insertRow(i + 1);
        row.setAttribute("id", "row" + i);
        for (let j = 0; j < 4; j++) {
            const cellName = colNames[j];
            const cell = row.insertCell(j);
            const path = patientLocations[i];
            cell.innerHTML = path[cellName];
        }
        if (allowEdit == true) {
            const cancle = row.insertCell(4);
            cancle.innerHTML = "    X ";
            cancle.addEventListener('click', removePath);
        }

    }
}
function addNewLocation() {
    const newPath = {
        startDate: document.getElementById("startDate").value,
        endDate: document.getElementById("endDate").value,
        city: document.getElementById("city").value,
        location: document.getElementById("location").value,
        patientId: document.getElementById('patientID').value
    };
    locations.push(newPath);
    locationsForPatient();
    cleanAddingOption();
    document.getElementById("save").value = "save";
}
function removePath(path) {
    const rowRemove = path.path[1].id.substr(3, 1);
    const patient = patientLocations[rowRemove];
    locations.splice(locations.indexOf(patient), 1);
    locationsForPatient();
    document.getElementById("save").value = "save";
}
function addAddingOption() {
    added = true;
    const AddStartDate = document.createElement("INPUT");
    AddStartDate.setAttribute("type", "date");
    AddStartDate.setAttribute("id", "startDate");
    document.getElementById("addLocation").appendChild(AddStartDate);

    const AddEndDate = document.createElement("INPUT");
    AddEndDate.setAttribute("type", "date");
    AddEndDate.setAttribute("id", "endDate");
    document.getElementById("addLocation").appendChild(AddEndDate);

    const AddCity = document.createElement("INPUT");
    AddCity.setAttribute("type", "text");
    AddCity.setAttribute("id", "city");
    document.getElementById("addLocation").appendChild(AddCity);

    const AddLocation = document.createElement("INPUT");
    AddLocation.setAttribute("type", "text");
    AddLocation.setAttribute("id", "location");
    document.getElementById("addLocation").appendChild(AddLocation);

    const addPath = document.createElement("INPUT");
    addPath.setAttribute("type", "button");
    addPath.setAttribute("id", "addPath");
    addPath.addEventListener("click", addNewLocation);
    addPath.value = "Add Location";
    document.getElementById("addLocation").appendChild(addPath);

    const saveLocations = document.createElement("INPUT");
    saveLocations.setAttribute("type", "button");
    saveLocations.setAttribute("id", "save");
    saveLocations.addEventListener("click", saveChanges);
    saveLocations.value = "Save";
    document.getElementById("saveLocations").appendChild(saveLocations);
}
function cleanTable() {
    const table = document.getElementById("pathsTable");
    table.innerHTML = "";
}
function cleanAddingOption() {
    document.getElementById('startDate').value = "";
    document.getElementById('endDate').value = "";
    document.getElementById('endDate').value = "";
    document.getElementById('city').value = "";
    document.getElementById('location').value = "";
}
initList(locations);
document.getElementById('cityInput').addEventListener("change", filterCity);
document.getElementById('select').addEventListener("change", filterCity);
document.getElementById('send').addEventListener("click", searchByDate );
function initList(listToInit) {
    document.getElementById('list').innerHTML = '';
    let sorted = sortDates(listToInit);
    sorted.forEach(element => {
        let item = document.createElement("li", element);
        item.innerText = element.startDate + ' ' + element.endDate + ' | ' + element.city + ' | ' + element.location
        document.getElementById('list').appendChild(item);
    });
}
function filterCity() {
    let selected = this.value;
    let match = getListFromServer(selected);
    initList(match);
}
function compareDates(a, b) {
    return a.startDate > b.startDate ? 1 : -1;
}
function sortDates(listToSort) {
    return listToSort.sort(compareDates);
}
function getLocationByPatientId() {
    console.log(added);
    const patientid = document.getElementById('patientID').value;
    document.getElementById("age").value = "";
    if (patientid === "" || patientid === null) {
        cleanTable();
        added = false;
        document.getElementById("addLocation").innerHTML = "";
        document.getElementById("saveLocations").innerHTML = "";
    }
    else {
        const xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                cleanTable();
                const jResponse = JSON.parse(this.responseText);
                const jLocations = jResponse["locations"];
                if (jLocations.length > 0) {
                    patientLocations.splice(0, patientLocations.length);
                    patientLocations.push(...jLocations);
                    createPathTable(patientLocations, true);
                }
                if (added === false)
                    addAddingOption();
            }
            if (this.status === 404 || this.response === null) {
                cleanTable();
                if (added === false) {
                    addAddingOption();
                }
            }
        };
        const url = BASICURL + "patient/" + patientid;
        xhr.open("GET", url, true);
        xhr.send();
    }
}
function getLocationByAge() {
    const age = document.getElementById("age").value;
    document.getElementById("addLocation").innerHTML = "";
    document.getElementById("saveLocations").innerHTML = "";
    document.getElementById("patientID").value = "";
    added = false;
    if (age === "" && age === null) {
        cleanTable();
       
    }
    else {
        const xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 || this.status == 200) {
                cleanTable();
                const jResponse = JSON.parse(this.responseText);

                if (jResponse.length > 0) {
                    patientLocations.splice(0, patientLocations.length);
                    patientLocations.push(...jResponse);
                    createPathTable(patientLocations,false);
                }

            }
            if (this.status === 404 || this.response === null) {
                cleanTable();
            }
        };
        const url = BASICURL + "LocationSearch?locationSearch.age=" + age;
        xhttp.open("GET", url, "true");
        xhttp.send();
    }
    console.log(age);
}

//function save -return patient

//function saveChangesAsync() {
//    let promise = new Promise(function (resolve, reject) {
//        var xhttp = new XMLHttpRequest();
//        xhttp.onreadystatechange = function () {
//            if (this.readyState == 4 && this.status == 200) {
//                document.getElementById("save").value = "saved !";
//                const jResponse = JSON.parse(this.responseText);
//            }
//        };
//        const body = {
//            patientId: document.getElementById('patientID').value,
//            locations=patientLocations
//        }
//        xhttp.open("POST", BASICURL + "patient", true);
//        xhttp.setRequestHeader("Content-Type", "application/json;charset=utf-8");
//        xhttp.setRequestHeader("Access-Control-Allow-Origin", "*");
//        xhttp.send(JSON.stringify(body));

//        resolve(jResponse);
//        reject(new Error("there aren't"));
//    });
//    promise.then(function (result) {
//        const id = document.getElementById('patientID').value;
//        locations = locations.filter(item => (item.patientId != id));
//        const l = result["locations"];
//        locations.push(...l);
//    });
//}

function saveChanges() {
    var xhttp = new XMLHttpRequest();
    const id = document.getElementById('patientID').value;
    const body = {
        id: id,
        locations: patientLocations
    }
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            document.getElementById("save").value = "saved !";
            const id = document.getElementById('patientID').value;
            locations = locations.filter(item => (item.patientId != id));
            locations.push(...patientLocations);
        }
    };
    xhttp.open("POST", BASICURL + "patient", true);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=utf-8");
    xhttp.setRequestHeader("Access-Control-Allow-Origin", "*");
    xhttp.send(JSON.stringify(body));
}
function loadServerResponse(list) {
    const jLocations = JSON.parse(list);
    if (jLocations.length > 0) {
        locations.splice(0, patientLocations.length);
        locations.push(...jLocations)
        initList(jLocations)
    }
}
function getListFromServer(city = "") {
    if (city != "") city = "?locationSearch.city=" + city;
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            loadServerResponse(this.responseText);
        }
    };
    xhttp.open("GET", BASICURL + "location" + city, true);
    xhttp.send();
}
function searchByDate() {
    const startDate = document.getElementById('startDate').value;
    const endDate = document.getElementById('endDate').value;
    const body = {
        startDate: startDate,
        endDate: endDate
    }
    const xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            alert("succes" + this.responseText);
            loadServerResponse(this.responseText);
        }
    };
    xhttp.open("POST", BASICURL + "LocationSearch", true);  
    xhttp.setRequestHeader("Content-Type", "application/json;charset=utf-8");
    xhttp.send(JSON.stringify(body));

}
