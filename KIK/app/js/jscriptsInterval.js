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