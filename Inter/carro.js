function mostrarCarrito() {
  const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
  const contenedor = document.getElementById('carritoListado');

  if (carrito.length === 0) {
    contenedor.innerHTML = "<p>No hay productos en el carrito.</p>";
    return;
  }

  let total = 0;

  let tabla = `
    <table class="table table-bordered align-middle">
      <thead>
        <tr>
          <th>Producto</th>
          <th>Categoría</th>
          <th>Precio</th>
          <th>Cantidad</th>
          <th>Total</th>
          <th>Quitar</th>
        </tr>
      </thead>
      <tbody>
  `;

  carrito.forEach((p, index) => {
    const subtotal = p.price * p.cantidad;
    total += subtotal;

    tabla += `
      <tr>
        <td>${p.name}</td>
        <td>${p.category}</td>
        <td>Q${p.price.toFixed(2)}</td>
        <td>
          <input type="number" min="1" value="${p.cantidad}" class="form-control form-control-sm"
            onchange="actualizarCantidad(${index}, this.value)" />
        </td>
        <td>Q${subtotal.toFixed(2)}</td>
        <td>
          <button class="btn btn-outline-danger btn-sm" onclick="eliminarDelCarrito(${index})">
            <i class="bi bi-trash"></i>
          </button>
        </td>
      </tr>
    `;
  });

  tabla += `
      </tbody>
      <tfoot>
        <tr>
          <td colspan="4"><strong>Total</strong></td>
          <td colspan="2"><strong>Q${total.toFixed(2)}</strong></td>
        </tr>
      </tfoot>
    </table>
  `;

  contenedor.innerHTML = tabla;
}

function eliminarDelCarrito(index) {
  const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
  carrito.splice(index, 1);
  localStorage.setItem("carrito", JSON.stringify(carrito));
  mostrarCarrito();
}

function actualizarCantidad(index, nuevaCantidad) {
  const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
  nuevaCantidad = parseInt(nuevaCantidad);
  if (nuevaCantidad > 0) {
    carrito[index].cantidad = nuevaCantidad;
    localStorage.setItem("carrito", JSON.stringify(carrito));
    mostrarCarrito();
  }
}

mostrarCarrito();
