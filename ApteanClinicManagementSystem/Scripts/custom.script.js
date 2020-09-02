

$(document).ready(function () {
    $('#pageTable').DataTable();
    $('.pagedTable').DataTable();
});


$("#menu-toggle").click(function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});


var error = document.getElementById('errorMessage');
if (error && error.innerHTML === "Medicine already exist")
    error.style.color = 'red';


$("AppointmentDatePicker").ready(function () {
    $('input[type=datetime]').datepicker({
        dateFormat: "dd/M/yy",
        changeMonth: true,
        changeYear: true,
        yearRange: "-0:+1",
        minDate: -0,
        maxDate: "+2M +0D",
        defaultDate: 15
    });

});
