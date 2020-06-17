import xhttp from './xhttp.js';
//import location from './location';
//import patient from './patient';

let added = false;

function getLocationByPatientId() {
    document.getElementById("age").value = '';
    xhttp.get("patient").then(
        resolve => sucsses(resolve),
        reject => failed(reject));

    function sucsses(dataFromServer) {
        cleanTable();
        const jLocations = dataFromServer["locations"];
        if (jLocations.length > 0) {
            patient.patientLocations.splice(0, patient.patientLocations.length);
            patient.patientLocations.push(...jLocations);
            createPathTable(patient.patientLocations, true);
        }
        if (added === false)
            addAddingOption();
    }
    function failed(error) {
        if (error.status == 401) {
            document.getElementById("message").innerHTML = "Authentication Error , please log-in!"
        }
        if (error.status === 404 || error.response === null) {
            cleanTable();
            if (added === false) {
                addAddingOption();
            }
        }
    };
}
//}class yt {
//    static async getLocationByPatientId() {
//        document.getElementById("age").value = '';
//        //xhttp.get("patient").then(
//        //    resolve => sucsses(resolve),
//        //    reject => failed(reject))
//        try {
//            const dataFromServer = await xhttp.get("patient");
//            sucsses(dataFromServer);
//        }
//        catch (e) {
//            failed(e);
//        }


//        function sucsses(dataFromServer) {
//            cleanTable();
//            const jLocations = dataFromServer["locations"];
//            if (jLocations.length > 0) {
//                patient.patientLocations.splice(0, patient.patientLocations.length);
//                patient.patientLocations.push(...jLocations);
//                createPathTable(patient.patientLocations, true);
//            }
//            if (added === false)
//                addAddingOption();
//        }
//        function failed(error) {
//            if (error.status == 401) {
//                document.getElementById("message").innerHTML = "Authentication Error , please log-in!"
//            }
//            if (error.status === 404 || error.response === null) {
//                cleanTable();
//                if (added === false) {
//                    addAddingOption();
//                }
//            }
//        };

//    }
//}
const searchBottun = document.getElementById('search');
searchBottun.addEventListener("click", getLocationByPatientId);
document.getElementById("searchAge").addEventListener("click", getLocationByAge);
document.getElementById("login").addEventListener("click", login);
document.getElementById('send').addEventListener("click", searchByDate);
document.getElementById('register').addEventListener("click", addRegisterPanel);
const helloTitle = document.createElement('h1');
helloTitle.innerText = 'Epidemiology Report';

document.getElementById("title").appendChild(helloTitle);
//onInit();
function locationsForPatient() {
    cleanTable();
    const patientLocations = patient.locationsForPatient(document.getElementById('patientID').value);
    if (patientLocations.length > 0)
        createPathTable(patientLocations);
    if (added === false)
        addAddingOption();
}
function createPathTable(patientLocations, allowEdit) {
    const table = document.getElementById("pathsTable");
    const row = table.insertRow(0);
    const colNames = Object.getOwnPropertyNames(patient.patientLocations[0]);
    const numCol = allowEdit === true ? 5 : 4;
    for (let i = 0; i < numCol; i++) {
        const cell = row.insertCell(i);
        if (i !== 4) {
            cell.innerHTML = colNames[i];
        }
        cell.setAttribute("class", "title");
    }
    for (let i = 0; i < patient.patientLocations.length; i++) {
        const row = table.insertRow(i + 1);
        row.setAttribute("id", "row" + i);
        for (let j = 0; j < 4; j++) {
            const cellName = colNames[j];
            const cell = row.insertCell(j);
            const path = patient.patientLocations[i];
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
    patient.locations.push(newPath);
    locationsForPatient();
    cleanAddingOption();
    document.getElementById("save").value = "save";
}
function removePath(path) {
    const rowRemove = path.path[1].id.substr(3, 1);
    const patient = patient.patientLocations[rowRemove];
    patient.locations.splice(patient.locations.indexOf(patient), 1);
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
function compareDates(a, b) {
    return a.startDate > b.startDate ? 1 : -1;
}
function sortDates(listToSort) {
    return listToSort.sort(compareDates);
}
function onInit() {
    const mytoken = xhttp.getCookie("token");
    token = mytoken;
    if (token !== "")
        getLocationByPatientId();
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
        xhttp.get(`Location?locationSearch.age= ${age}`)
            .then(resolve => sucsses(resolve),
                reject => cleanTable())
        function sucsses(data) {
            cleanTable();
            const jResponse = JSON.parse(this.responseText);
            if (jResponse.length > 0) {
                patientLocations.splice(0, patientLocations.length);
                patientLocations.push(...jResponse);
                createPathTable(patientLocations, false);
            }

        }
    };
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
    const body = {
        //id: id,הסרבר אמור לקחת את זה מהטוקן 
        locations: patient.patientLocations
    }

    xhttp.post("patient", body).then(
        resolve => sucsses(resolve),
        reject => console.log(`save changes failed" ${reject}`)
    );
    function sucsses(data) {

        document.getElementById("save").value = "saved !";
        const id = data["id"]//משהו כזה..
        patient.locations = patient.locations.filter(item => (item.patientId != id));
        patient.locations.push(...patient.patientLocations);
    }
};
function login() {
    document.getElementById("message").innerHTML = ""
    const name = document.getElementById("name").value;
    const password = document.getElementById("password").value;
    const authModel = {
        name: name,
        password: password
    }
    xhttp.post("patient/Authenticate", authModel).then(
        resolve => sucsses(resolve),
        reject => failed(reject)
    )
    function sucsses(data) {
        const token = data["token"];
        document.cookie = `token= ${token}`;
        console.log(token);
        document.getElementById("messageLogin").innerHTML = "login successed ! ";
        document.getElementById("messageLogin").style.color = "blue";
        getUserName();
        getLocationByPatientId();
        getListFromServer();
    }
    function failed(error) {
        if (error.status === 400) {
            document.getElementById("messageLogin").innerHTML = "Authentication Error , please try again or register !"
            document.getElementById("messageLogin").style.color = "red";
        }
        else if (error.status == 401) {
            document.getElementById("messageLogin").innerHTML = "Authentication Error , please try again or register !"
            document.getElementById("messageLogin").style.color = "red";
        }
        console.error(error.response);
    };
}
function register() {
    const body = {
        name: document.getElementById("userName").value,
        password: document.getElementById("userPassword").value,
        id: document.getElementById("userIdentity").value
    }
    xhttp.post("patient/register", body).then(
        resolve => sucsses(resolve),
        reject => faied(reject))
    function sucsses(data) {
        token = data["token"];
        document.cookie = `token= ${token}`;
        console.log(token);
        alert("register successed ! ");
        getUserName();
        document.getElementById("messageLogin").innerHTML = "register successed ! ";
        document.getElementById("messageLogin").style.color = "blue";
    }
    function failed(error) {
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
        console.error(error.response);
    }
}
function getUserName() {
    xhttp.get("patient/username").then(
        resolve => document.getElementById("patientName").innerHTML = "Hello " + resolve)
}
function loadServerResponse(list) {
    if (list.length > 0) {
        patient.locations.splice(0, patient.patientLocations.length);
        patient.locations.push(...list);
        initList(list);
    }
}
function getListFromServer() {
    xhttp.get("location").then(
        resolve => loadServerResponse(resolve))
}
function searchByDate() {
    const body = {
        city: document.getElementById('select').value,
        startDate: document.getElementById('startDate').value,
        endDate: document.getElementById('endDate').value
    }
    xhttp.post("Location", body).then(
        resolve => loadServerResponse(resolve))
}