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
          <th>Acciones</th>
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
        <td>${p.cantidad}</td>
        <td>Q${subtotal.toFixed(2)}</td>
        <td><button class="btn btn-danger btn-sm" onclick="eliminarDelCarrito(${index})">Eliminar</button></td>
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
  carrito.splice(index, 1); // Elimina 1 elemento en la posición index
  localStorage.setItem("carrito", JSON.stringify(carrito));
  mostrarCarrito(); // Recarga el carrito
}

mostrarCarrito();
