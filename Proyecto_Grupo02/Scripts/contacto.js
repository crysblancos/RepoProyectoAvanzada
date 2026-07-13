document.addEventListener("DOMContentLoaded", function () {

    const formulario = document.getElementById("formContacto");
    const boton = document.getElementById("btnEnviarContacto");
    const telefono = document.getElementById("contactoTelefono");

    if (!formulario || !boton || !telefono) {
        return;
    }

    function validarTelefono() {

        const formatoTelefono = /^\d{4}-\d{4}$/;

        if (!formatoTelefono.test(telefono.value.trim())) {
            telefono.setCustomValidity(
                "Ingrese el teléfono con el formato 8888-8888."
            );
        } else {
            telefono.setCustomValidity("");
        }
    }

    telefono.addEventListener("input", function () {

        let valor = telefono.value.replace(/\D/g, "");

        if (valor.length > 4) {
            valor = valor.substring(0, 4) + "-" + valor.substring(4, 8);
        }

        telefono.value = valor;

        validarTelefono();
    });

    boton.addEventListener("click", function () {

        validarTelefono();

        formulario.classList.add("was-validated");

        if (!formulario.checkValidity()) {
            return;
        }

        alert("El formulario de contacto contiene todos los datos requeridos.");
    });
});