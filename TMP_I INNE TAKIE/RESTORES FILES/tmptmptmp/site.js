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