document.addEventListener("DOMContentLoaded", function () {

    const telefono = document.getElementById("contactoTelefono");
    if (!telefono) {
        return;
    }

    telefono.addEventListener("input", function () {
        let valor = telefono.value.replace(/\D/g, "");
        if (valor.length > 4) {
            valor = valor.substring(0, 4) + "-" + valor.substring(4, 8);
        }
        telefono.value = valor;
    });
});