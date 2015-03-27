
var num = null;
$(".btn-group > label.btn").on("click", function(){
    num = this.innerText;
    //alert("Value is " + num);
    //ViewBag.entidad = num;
});