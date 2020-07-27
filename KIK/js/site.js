import { clearInterval } from "timers";

/*
function CheckEmailConfirmationStatusOLD(email) {
    $.get("/CheckEmailConfirmationStatys?email=" + email,
        function (data) {
            if (data === "OK") {
                if (interval !== null) {
                    clearInterval(interval);
                }
                window.location.href = "/GameInvitation?email=" + email;
            }
        });
}
*/

function CheckEmailConfirmationStatus(email) {
    $.get("/CheckEmailConfirmationStatys?email=" + email,
        function (data) {
            if (data === "OK") {
                if (interval !== null) {
                    clearInterval(interval);
                }
                window.location.href = "/GameInvitation?email=" + email;
            }
        });
}

    var openSocket = function (parameter, strAction) {
        if (interval != null) {
            clearInterval(interval);
        }

        var protocol = location.protocol === "https:" ? "wss:" ? "ws:";

        var operation = "";
        var wsUri = "";

        if (strAction == "Email") {
            wsUri = protocol + "//" + window.location.host + "/CheckEmailConfirmationStatus";
        }

        var socket = new WebSocket(wsUri);
        socket.onmessage = function (respose) {
            console.log(respose);
            if (strAction == "Email" && respose.data == "OK") {
                window.location.href = "/GameInvitation?email=" + parameter;
            }
        };

        socket.onopen = function () {
            var json = JSON.stringify({
                "operation": operation,
                "parameter": parameter
            });
            socket.send(json);
        };
        socket.onclose = function (event) {

        };


    };


/*
 var interval;
function EmailConfirmationOLD(email) {
    interval = setInterval(() => {
        CheckEmailConfirmationStatus(email);
    }, 5000);
}
*/

var interval;
function EmailConfirmation(email) {
    if (window.WebSocket) {
        alert("Websockets re activated");
        openSocket(email, "Email");
    }
    else {
        alert("Websocket unavailable")
        interval = setInterval(() => {
            CheckEmailConfirmationStatus(email);
        }, 5000);
    }
}