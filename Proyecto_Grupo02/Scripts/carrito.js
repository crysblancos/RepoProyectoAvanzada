document.addEventListener("DOMContentLoaded", function () {

    const listaCarrito = document.getElementById("listaCarrito");

    if (!listaCarrito) {
        return;
    }

    const costoEntrega = 2500;

    const carritoVacio = document.getElementById("carritoVacio");
    const cantidadProductos = document.getElementById("cantidadProductos");
    const subtotalCarrito = document.getElementById("subtotalCarrito");
    const totalCarrito = document.getElementById("totalCarrito");
    const btnContinuarPedido = document.getElementById("btnContinuarPedido");

    function formatearColones(valor) {
        return "₡" + valor.toLocaleString("es-CR", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
    }

    function obtenerProductos() {
        return document.querySelectorAll(".ka-cart-item");
    }

    function actualizarCarrito() {

        const productos = obtenerProductos();

        let subtotal = 0;
        let cantidadTotal = 0;

        productos.forEach(function (producto) {

            const precio = Number(
                producto.getAttribute("data-precio")
            );

            const inputCantidad =
                producto.querySelector(".cantidad-producto");

            const cantidad = Number(inputCantidad.value);

            subtotal += precio * cantidad;
            cantidadTotal += cantidad;
        });

        if (subtotalCarrito) {
            subtotalCarrito.textContent =
                formatearColones(subtotal);
        }

        if (totalCarrito) {

            const total =
                productos.length > 0
                    ? subtotal + costoEntrega
                    : 0;

            totalCarrito.textContent =
                formatearColones(total);
        }

        if (cantidadProductos) {
            cantidadProductos.textContent =
                cantidadTotal === 1
                    ? "1 artículo"
                    : cantidadTotal + " artículos";
        }

        if (productos.length === 0) {

            listaCarrito.classList.add("d-none");

            if (carritoVacio) {
                carritoVacio.classList.remove("d-none");
            }

            if (btnContinuarPedido) {
                btnContinuarPedido.disabled = true;
            }

        } else {

            listaCarrito.classList.remove("d-none");

            if (carritoVacio) {
                carritoVacio.classList.add("d-none");
            }

            if (btnContinuarPedido) {
                btnContinuarPedido.disabled = false;
            }
        }
    }

    document.addEventListener("click", function (evento) {

        const botonSumar =
            evento.target.closest(".btn-sumar");

        const botonRestar =
            evento.target.closest(".btn-restar");

        const botonEliminar =
            evento.target.closest(".ka-delete-button");

        if (botonSumar) {

            const producto =
                botonSumar.closest(".ka-cart-item");

            const input =
                producto.querySelector(".cantidad-producto");

            input.value = Number(input.value) + 1;

            actualizarCarrito();
        }

        if (botonRestar) {

            const producto =
                botonRestar.closest(".ka-cart-item");

            const input =
                producto.querySelector(".cantidad-producto");

            if (Number(input.value) > 1) {
                input.value = Number(input.value) - 1;
            }

            actualizarCarrito();
        }

        if (botonEliminar) {

            const producto =
                botonEliminar.closest(".ka-cart-item");

            producto.remove();

            actualizarCarrito();
        }
    });

    actualizarCarrito();
});