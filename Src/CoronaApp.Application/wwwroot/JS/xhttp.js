export default class xhttp {
    static BASICURL() {
        return "https://localhost:44381/api/";
    }
    static token() {
        return xhttp.getCookie("token");
    }
//    static try() {
//        al
//}
    static getCookie(cookieName) {
        var name = cookieName + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
    static get(url) {
        const http = new XMLHttpRequest();
        return new Promise((resolve, reject) => {
            http.onreadystatechange = function () {
                if (this.readyState === 4 || this.status === 200) {
                    resolve(JSON.parse(this.responseText));

                }
                if (this.readyState == 4 || this.status !== 200) {
                    reject({ statusCode: this.status, response: JSON.parse(this.responseText) });
                }
            };
            http.open("GET", `${xhttp.BASICURL()}${url}`, "true");
            http.setRequestHeader('Authorization', `Bearer ${xhttp.token()}`);
            http.send();
        })
    }
    static post(url, body) {
        return new Promise(function (resolve, reject) {
            var http = new XMLHttpRequest();
            http.onreadystatechange = function () {
                if (this.readyState === 4 && this.status === 200) {
                    resolve(JSON.parse(this.responseText));
                }
                if (this.readyState === 4 && this.status !== 200) {
                    reject({ statusCode: this.status, response: JSON.parse(this.responseText) });
                }
            };
            const base = "https://localhost:44381/api/";
            http.open("POST", `${base}${url}`, true);
            http.setRequestHeader("Content-Type", "application/json;charset=utf-8");
            http.setRequestHeader("Access-Control-Allow-Origin", "*");
            const token = xhttp.token();
            if (token !== "") {
                http.setRequestHeader('Authorization', `Bearer ${token}`);
            }
            http.send(JSON.stringify(body));
        });
    }
}