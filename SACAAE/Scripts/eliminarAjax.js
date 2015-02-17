function grupoAjax() {

    var route = "/Groups/Grupos/List/" + $('select[name="selectPlan"]').val() + "/" + $('select[name="selectBloque"]').val() +
                        "/" + $('select[name="selectSede"]').val();
    $.getJSON(route, function (data) {
        var items = "";
        $.each(data, function (i, plan) {
            items += "<option value='" + plan.Value + "'>" + plan.Text + "</option>";
        });
        $("#selectGrupo").html(items);

    });
}



$(function () {
    $.getJSON("/Groups/Sedes/List", function (data) {
        var items;
        $.each(data, function (i, sede) {
            items += "<option value='" + sede.Value + "'>" + sede.Text + "</option>";
        });
        $("#selectSede").html(items);
        var items = "";
        for (var i = 1; i < 12; i++) {
            items += "<option value='" + i + "'>" + i + "</option>";
        }
        $("#selectBloque").html(items);
    });
    $.getJSON("/Groups/Modalidades/List", function (data) {
        var items;
        $.each(data, function (i, modalidad) {
            items += "<option value='" + modalidad.Value + "'>" + modalidad.Text + "</option>";
        });
        $("#selectModalidad").html(items);
      
    });

    $("#selectSede").change(function () {

        var route = "/Groups/Planes/List/" + $('select[name="selectModalidad"]').val() + "/" + $('select[name="selectSede"]').val();

        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, plan) {

                items += "<option value='" + plan.Value + "'>" + plan.Text + "</option>";
            });
            $("#selectPlan").html(items);
        });
       
       $("#selectGrupo").html(" ");
    });
  

    $("#selectModalidad").change(function () {

        var route = "/Groups/Planes/List/" + $('select[name="selectModalidad"]').val() + "/" + $('select[name="selectSede"]').val();

        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, plan) {

                items += "<option value='" + plan.Value + "'>" + plan.Text + "</option>";
            });
            $("#selectPlan").html(items);
            
            $("#selectGrupo").html(" ");
        });
        
    });

});

