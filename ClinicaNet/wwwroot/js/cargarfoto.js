$(document).ready(function () {

    $("#seleccionImg").change(function () { //Este evento se disparara cuando el usuario seleccione un archivo

        var tam = this.files[0].size; //Capta el tamaño del archivo de la foto

        if (tam > 5000000) { // Si el tamaño supera esto, falla.
            alert("El tamaño del archivo no debe ser mayor a 5 mb");
        }
        else {
            readURL(this); /* Si el archivo es válido, se ejecuta la función readURL y el this hace referencia al elemento que ha disparado el evento.
            En este caso, es el campo de entrada de archivo (<input type="file">), NO LA IMAGEN, el elemento input en el que el usuario seleccionó la 
            imagen.*/
        }
    });
});
function readURL(input) {

    if (input.files && input.files[0]) { //Verifica si se han seleccionado archivos y si hay al menos un archivo en la lista de archivos seleccionados

        var reader = new FileReader(); 

        reader.onload = function (e) {
            $("#imagen").attr("src", e.target.result);
        }

        reader.readAsDataURL(input.files[0]); //lee como un string en base64
    }
}