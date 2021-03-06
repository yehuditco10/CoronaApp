﻿import User from './xhttp.js';
const y = new User("kili");
alert(y.name);
let locations = [];
const patientLocations = [];
let added = false;
let token = "";
const searchBottun = document.getElementById('search');
searchBottun.addEventListener("click", getLocationByPatientId);
document.getElementById("searchAge").addEventListener("click", getLocationByAge);
document.getElementById("login").addEventListener("click", login);
//document.getElementById('select').addEventListener("change", filterCity);
document.getElementById('send').addEventListener("click", searchByDate);
document.getElementById('register').addEventListener("click", addRegisterPanel);
const helloTitle = document.createElement('h1');
helloTitle.innerText = 'Epidemiology Report';

document.getElementById("title").appendChild(helloTitle);
initList(locations);
onInit();
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
function createPathTable(patientLocations, allowEdit) {
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
function addRegisterPanel() {
    document.getElementById("loginPanel").innerHTML = "";
    const userNameInput = document.createElement("input");
    userNameInput.setAttribute("type", "text");
    userNameInput.setAttribute("id", "userName");
    document.getElementById("loginPanel").appendChild(userNameInput);

    const passwordInput = document.createElement("input");
    passwordInput.setAttribute("type", "password");
    passwordInput.setAttribute("id", "userPassword");
    document.getElementById("loginPanel").appendChild(passwordInput);

    const userIdInput = document.createElement("input");
    userIdInput.setAttribute("type", "text");
    userIdInput.setAttribute("id", "userIdentity");
    document.getElementById("loginPanel").appendChild(userIdInput);

    const registerInput = document.createElement("input");
    registerInput.setAttribute("type", "button");
    registerInput.setAttribute("id", "registerUser");
    registerInput.setAttribute("value", "register now");
    registerInput.addEventListener("click", register);
    document.getElementById("loginPanel").appendChild(registerInput);
}
function initList(listToInit) {
    const list = document.getElementById('list');
    list.innerHTML = '';
    if (listToInit !== undefined) {
        let sorted = sortDates(listToInit);
        sorted.forEach(element => {
            let item = document.createElement("li", element);
            item.innerText = element.startDate + '  |  ' + element.endDate + '  |  ' + element.city + '  |  ' + element.location
            list.appendChild(item);
        });
    }
    else
        list.innerHTML = 'No data match to your search!'
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
function onInit() {
    const mytoken = getCookie("token");
    token = mytoken;
    if (token !== "")
        getLocationByPatientId();
}
function getLocationByPatientId() {
    document.getElementById("age").value = "";
    //if (patientid === "" || patientid === null) {
    //    cleanTable();
    //    added = false;
    //    document.getElementById("addLocation").innerHTML = "";
    //    document.getElementById("saveLocations").innerHTML = "";
    //}
    //else {
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
        if (this.status == 401) {
            document.getElementById("message").innerHTML = "Authentication Error , please log-in!"
        }
        if (this.status === 404 || this.response === null) {
            cleanTable();
            if (added === false) {
                addAddingOption();
            }
        }
    };
    const url = BASICURL + "patient";
    xhr.open("GET", url, true);

    xhr.setRequestHeader('Authorization', `Bearer ${token}`);
    xhr.send();
    //   }
}
function getLocationByAge() {
    const age = document.getElementById("age").value;
    document.getElementById("addLocation").innerHTML = "";
    document.getElementById("saveLocations").innerHTML = "";
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
                    createPathTable(patientLocations, false);
                }

            }
            if (this.status === 404 || this.response === null) {
                cleanTable();
            }
        };
        const url = BASICURL + "Location?locationSearch.age=" + age;
        xhttp.open("GET", url, "true");
        xhttp.setRequestHeader('Authorization', `Bearer ${token}`);
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

//todo autorize
function saveChanges() {
    var xhttp = new XMLHttpRequest();
    //const id = document.getElementById('patientID').value;
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
    xhr.setRequestHeader('Authorization', `Bearer ${token}`);
    xhttp.send(JSON.stringify(body));
}
//function getCookie(cookieName) {
//    var name = cookieName + "=";
//    var decodedCookie = decodeURIComponent(document.cookie);
//    var ca = decodedCookie.split(';');
//    for (var i = 0; i < ca.length; i++) {
//        var c = ca[i];
//        while (c.charAt(0) == ' ') {
//            c = c.substring(1);
//        }
//        if (c.indexOf(name) == 0) {
//            return c.substring(name.length, c.length);
//        }
//    }
//    return "";
//}
function login() {

    document.getElementById("message").innerHTML = ""
    const name = document.getElementById("name").value;
    const password = document.getElementById("password").value;
    var xhttp = new XMLHttpRequest();
    const authModel = {
        name: name,
        password: password
    }
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            const jResponse = JSON.parse(this.responseText);
            token = jResponse["token"];
            document.cookie = "token=" + token;
            console.log(token);
            document.getElementById("messageLogin").innerHTML = "login successed ! ";
            document.getElementById("messageLogin").style.color = "blue";
            getUserName();
            getLocationByPatientId();
            getListFromServer()
        }
        //??
        else if (this.status == 400) {
            document.getElementById("messageLogin").innerHTML = "Authentication Error , please try again or register !"
            document.getElementById("messageLogin").style.color = "red";
        }
        else if (this.status == 401) {
            document.getElementById("messageLogin").innerHTML = "Authentication Error , please try again or register !"
            document.getElementById("messageLogin").style.color = "red";
        }

    };
    xhttp.open("POST", BASICURL + "patient/Authenticate", true);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=utf-8");
    xhttp.setRequestHeader("Access-Control-Allow-Origin", "*");

    var json = JSON.stringify(authModel)
    xhttp.send(json);
    //xhttp.send(authModel);
}
function register() {
    const xhttp = new XMLHttpRequest();
    const body = {
        name: document.getElementById("userName").value,
        password: document.getElementById("userPassword").value,
        id: document.getElementById("userIdentity").value
    }
    xhttp.onreadystatechange = function () {

        if (this.status == 200) {
            const jResponse = JSON.parse(this.responseText);
            token = jResponse["token"];
            console.log(token);
            alert("register successed ! ");
            getUserName();

            //document.getElementById("messageLogin").innerHTML = "register successed ! ";
            //document.getElementById("messageLogin").style.color = "blue";
        }
        //??
        //else if (this.status == 400) {
        //    document.getElementById("messageLogin").innerHTML = "Authentication Error , please try again or register !"
        //    document.getElementById("messageLogin").style.color = "red";
        //}
        //else if (this.status == 401) {
        //    document.getElementById("messageLogin").innerHTML = "Authentication Error , please try again or register !"
        //    document.getElementById("messageLogin").style.color = "red";
        //}
        //else {
        //    document.getElementById("messageLogin").innerHTML = "register failed"
        //    document.getElementById("messageLogin").style.color = "red";
        //}
    };
    xhttp.open("POST", BASICURL + "patient/register", true);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=utf-8");
    xhttp.setRequestHeader("Access-Control-Allow-Origin", "*");
    xhttp.send(JSON.stringify(body));
}
function getUserName() {
    const xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.status == 200) {
            // const jResponse = JSON.parse(this.responseText);
            document.getElementById("patientName").innerHTML = "Hello " + this.responseText;
        }
        if (this.status === 404 || this.response === null) {

        }
    }
    const url = BASICURL + "patient/username";
    xhttp.open("GET", url, "true");
    userToken = getCookie('token');
    xhttp.setRequestHeader('Authorization', `Bearer ${userToken}`);
    xhttp.send();
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
    xhttp.setRequestHeader("Authorization", `Bearer ${token}`);
    xhttp.send();
}
function searchByDate() {
    const startDate = document.getElementById('startDate').value;
    const endDate = document.getElementById('endDate').value;
    const city = document.getElementById('select').value;
    const body = {
        city: city,
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
    xhttp.open("POST", BASICURL + "Location", true);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=utf-8");
    xhttp.setRequestHeader('Authorization', `Bearer ${token}`);
    xhttp.send(JSON.stringify(body));

}
