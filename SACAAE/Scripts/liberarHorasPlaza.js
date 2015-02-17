$(document).ready(function () {
    $("#HA").prop("disabled", "disabled");
    $("#HT").prop("disabled", "disabled");
    $("#sltPlaza").change(function () {        
        CargarProfe();

    });
    $("#sltProfesor").change(function () {
        CargarTextoHorasXLiberar();

    });
});

function CargarTextoHorasXLiberar() {
    var route = "/getPlazaXProfesor/HorasTotales/Una/" + $('select[name="sltPlaza"]').val() + "/" + $('select[name="sltProfesor"]').val();
    $.getJSON(route, function (data) {
        $("#HT").val(data[0]);
        $("#HA").val(data[1]);

    });
}

function validarHorasXLiberar() {
    var HorasTotales = $("#HT").val();
    var HorasAsignadas = $("#HA").val();
    var HorasPorAsignadas = $("#HPA").val();
    if (HorasAsignadas = 0) {
        alert("ERROR: No puede liberar horas, la cantidad es mayor a la disponible");
        return false;
    }
    else if (HorasPorAsignadas > (HorasTotales - HorasAsignadas)) {
        alert("ERROR: Debe liberar menos horas, la cantidad es mayor a la disponible");
        return false;
    }
    else
        return true;
}

function CargarProfe() {

    var route = "/getPlazaXProfesor/List/Profes/" + $("#sltPlaza").val();
    $.getJSON(route, function (data) {
        var items = "";
        if (data != "")
            $.each(data, function (i, profesor) {
                items = "<option value='" + profesor.ID + "'>" + profesor.Nombre + "</option>";
            });       
            
        $("#sltProfesor").html(items);
        CargarTextoHorasXLiberar();

    });


}