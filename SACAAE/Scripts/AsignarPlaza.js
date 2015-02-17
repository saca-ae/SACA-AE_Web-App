$(document).ready(function () {
    $("#HA").prop("disabled", "disabled");
    $("#HT").prop("disabled", "disabled");
    $("#sltPlaza").change(function () {
        CargarTextoHoras();
        
    });
});

function CargarTextoHoras()
{
    var route = "/getPlaza/HorasTotales/Una/" + $('select[name="sltPlaza"]').val();
    $.getJSON(route, function (data) {
        $("#HT").val(data[0]);
        $("#HA").val(data[1]);

    });
}

function validarHoras()
{
    var HorasTotales = $("#HT").val();
    var HorasAsignadas = $("#HA").val();
    var HorasPorAsignadas = $("#HPA").val();
    if (HorasPorAsignadas > (HorasTotales - HorasAsignadas))
    {
        alert("ERROR: Debe asignar menos horas, la cantidad es mayor a la disponible de la plaza");
        return false;
    }
        return true;
}