$(document).ready(function () { /* DOM ready */
    $('#Specializations').change(function () {
        var specialization = $(this).val();

        $.ajax({
            url: 'GetDoctors/',
            type: 'post',
            data: { specialization: specialization },
            dataType: 'json',
            success: function (response) {

                var len = response.length;

                $("#DoctorId").empty();
                $("#DoctorId").append("<option value=''>-- Select Doctor --</option>")
                for (var i = 0; i < len; i++) {
                    var id = response[i]['DoctorId'];
                    var name = response[i]['UserDetails']['FullName'];

                    $("#DoctorId").append("<option value='" + id + "'>" + name + "</option>");

                }
            }
        });
    });

    $("#DoctorId").change(function () {
        GetTimeSlots();
    });
    $('#Username').focusout(function () {
        GetTimeSlots();
    });
    $("#AppointmentDatePicker").datepicker().on("change", function () {
        GetTimeSlots();
    });

    function GetTimeSlots() {
        var doctor = $("#DoctorId").children("option:selected").val();
        var patient = $("#Username").val();
        var date = $("#AppointmentDatePicker").val();
        //var token = $('input[name="__RequestVerificationToken"]').val();
        if (typeof doctor == undefined || typeof patient == undefined || typeof date == undefined) {
            return;
        }
        $.ajax({
            url: 'GetTimeSlots/',
            type: 'post',
            data: {
                doctorId: doctor,
                patientUsername: patient,
                appointmentDate: date,
                //__RequestVerificationToken: token
            },
            dataType: 'json',
            success: function (response) {

                var len = response.length;

                $("#AppointmentTime").empty();
                $("#AppointmentTime").append("<option value=''>-- Select Appointment Time --</option>")
                for (var i = 0; i < len; i++) {
                    var time = response[i];

                    $("#AppointmentTime").append("<option>" + time + "</option>");

                }
            }
        });
    }
});