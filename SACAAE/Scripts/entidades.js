var num = null;
$(".btn-group > button.btn").on("click", function(){
    num = this.innerHTML;
    //alert("Value is " + num);
    ViewBag.entidad = num;
});