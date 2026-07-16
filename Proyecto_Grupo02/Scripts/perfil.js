document.addEventListener("DOMContentLoaded", function () {

    const formulario = document.getElementById("formPerfil");
    const boton = document.getElementById("btnGuardarPerfil");
    const contrasenna = document.getElementById("perfilContrasenna");
    const confirmar = document.getElementById("perfilConfirmar");

    if (!formulario || !boton || !contrasenna || !confirmar) {
        return;
    }

    boton.addEventListener("click", function () {

        confirmar.setCustomValidity("");

        if (contrasenna.value !== confirmar.value) {
            confirmar.setCustomValidity("Las contraseñas no coinciden.");
        }

        formulario.classList.add("was-validated");

        if (!formulario.checkValidity()) {
            return;
        }
    });

    confirmar.addEventListener("input", function () {

        if (contrasenna.value === confirmar.value) {
            confirmar.setCustomValidity("");
        }
    });
});