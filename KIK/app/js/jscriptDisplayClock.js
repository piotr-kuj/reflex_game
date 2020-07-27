function updateClock() {
    var currentTime = new Date();

    var currentHours = currentTime.getHours();
    var currentMinutes = currentTime.getMinutes();
    var currentSeconds = currentTime.getSeconds();

    // Pad the minutes and seconds with leading zeros, if required
    currentMinutes = (currentMinutes < 10 ? "0" : "") + currentMinutes;
    currentSeconds = (currentSeconds < 10 ? "0" : "") + currentSeconds;

    // Choose either "AM" or "PM" as appropriate
    var timeOfDay = (currentHours < 12) ? "AM" : "PM";

    // Convert the hours component to 12-hour format if needed
    currentHours = (currentHours > 12) ? currentHours - 12 : currentHours;

    // Convert an hours component of "0" to "12"
    currentHours = (currentHours == 0) ? 12 : currentHours;

    // Compose the string for display
    var currentTimeString = currentHours + ":" + currentMinutes + ":" + currentSeconds + " " + timeOfDay;

    // Update the time display
    document.getElementById("clock").style.color = "white";
    document.getElementById("clock").style.fontSize = "x-large";
    document.getElementById("clock").setAttribute("align", "center");

    document.getElementById("clock").firstChild.nodeValue = currentTimeString;
}

                                // -->



/*
$("time").each(function (elem) {
    var utctimeval = $(this).html();
    var date = new Date(utctimeval);
    $(this).html(date.toLocaleString());
})


                         <span id="clock">&nbsp;</span>
                        <script type="text/javascript">
<!--
                            function updateClock() {
                                var currentTime = new Date();

                                var currentHours = currentTime.getHours();
                                var currentMinutes = currentTime.getMinutes();
                                var currentSeconds = currentTime.getSeconds();

                                // Pad the minutes and seconds with leading zeros, if required
                                currentMinutes = (currentMinutes < 10 ? "0" : "") + currentMinutes;
                                currentSeconds = (currentSeconds < 10 ? "0" : "") + currentSeconds;

                                // Choose either "AM" or "PM" as appropriate
                                var timeOfDay = (currentHours < 12) ? "AM" : "PM";

                                // Convert the hours component to 12-hour format if needed
                                currentHours = (currentHours > 12) ? currentHours - 12 : currentHours;

                                // Convert an hours component of "0" to "12"
                                currentHours = (currentHours == 0) ? 12 : currentHours;

                                // Compose the string for display
                                var currentTimeString = currentHours + ":" + currentMinutes + ":" + currentSeconds + " " + timeOfDay;

                                // Update the time display
                                document.getElementById("clock").firstChild.nodeValue = currentTimeString;
                            }

                            // -->
                        </script>



////////////////////////////////////////////////////////////////////////////////////
layout
////////////////////////////////////////////////////////////////////////

                            <!--
                        <a href="#" class="navbar-brand" style="color:aliceblue;">
                           // @{
                                //while (true)
                                {
                                    //Task.Delay(999);
                                    //@DateTime.Now
                                    }
                                }
                        </a>


                        <a href="#" class="navbar-right">
        @model DateTime
        <span style="color:aliceblue;" class="UTCTime">@Model</span>
                        </a>
        -->


*/