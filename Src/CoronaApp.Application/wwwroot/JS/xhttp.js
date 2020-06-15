export default class xhttp {
     BASICURL = "https://localhost:44381/api/";
     token() {
        return getCookie("token");
    }
    static try() {
        al

}
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
        const xhttp = new XMLHttpRequest();
        return new Promise((resolve, reject) => {
            xhttp.onreadystatechange = function () {
                if (this.readyState === 4 || this.status === 200) {
                    resolve(JSON.parse(this.responseText));

                }
                if (this.readyState == 4 || this.status !== 200) {
                    reject({ statusCode: this.status, response: JSON.parse(this.responseText) });
                }
            };
            xhttp.open("GET", `${this.BASICURL()} ${url}`, "true");
            xhttp.setRequestHeader('Authorization', `Bearer ${this.token()}`);
            xhttp.send();
        })
    }
    static post(url, body) {
        return new Promise(function (resolve, reject) {
            var xhttp = new XMLHttpRequest();
            xhttp.onreadystatechange = function () {
                if (this.readyState === 4 && this.status === 200) {
                    resolve(JSON.parse(this.responseText));
                }
                if (this.readyState === 4 && this.status !== 200) {
                    reject({ statusCode: this.status, response: JSON.parse(this.responseText) });
                }
            };
            const base = "https://localhost:44381/api/" ;
            xhttp.open("POST", `${base} ${url}`, true);
            xhttp.setRequestHeader("Content-Type", "application/json;charset=utf-8");
            xhttp.setRequestHeader("Access-Control-Allow-Origin", "*");
            xhttp.setRequestHeader('Authorization', `Bearer ${this.token()}`);
            xhttp.send(JSON.stringify(body));
        });
    }
}