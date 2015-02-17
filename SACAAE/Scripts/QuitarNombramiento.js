$(document).ready(function () {
    $("#sltPlaza").change(function () {
        CargarProfe();
    });
});

function CargarProfe() {
    
        var route = "/getPlazaXProfesorNombrado/List/Profes/" + $("#sltPlaza").val();
        $.getJSON(route, function (data) {
            var items = "";
            if(data!="")
            items = "<option value='" + data[0].ID + "'>" + data[0].Nombre + "</option>";
                $("#sltProfesor").html(items);

        });

    
}