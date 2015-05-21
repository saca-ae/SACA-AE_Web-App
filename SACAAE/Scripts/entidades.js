function setEntidad(entidad) {

    Load(entidad);   
    location.reload();

}


function Load(entidad) {
  
    setCookie("Entidad", entidad, "/");
}


function setCookie(name, value, path) {
    var curCookie = name + "=" + value +
        ((path) ? "; path=" + path : "");
    document.cookie = curCookie;
}

