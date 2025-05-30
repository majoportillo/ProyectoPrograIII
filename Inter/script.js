const apiUrl = 'https://localhost:32769/api/Product';

async function cargarProductos() {
  try {
    const res = await fetch(apiUrl);
    if (!res.ok) throw new Error('Error HTTP: ' + res.status);
    const productos = await res.json();
    mostrarProductos(productos);
  } catch (err) {
    alert('Error al cargar productos: ' + err.message);
  }
}

function mostrarProductos(productos) {
  const contenedor = document.getElementById('productos');
  contenedor.innerHTML = '';

  productos.forEach(p => {
    const col = document.createElement('div');
    col.className = 'col';

    col.innerHTML = `
      <div class="card h-100 shadow-sm">
        ${p.imageUrl ? `<img src="${p.imageUrl}" class="card-img-top" alt="${p.name}" />` : ''}
        <div class="card-body d-flex flex-column">
          <h5 class="card-title">${p.name}</h5>
          <p class="card-text"><strong>Categoría:</strong> ${p.category}</p>
          <p class="card-text"><strong>Precio:</strong> Q${p.price}</p>
          <p class="card-text">${p.description}</p>
          <input type="number" class="form-control mb-2" id="cantidad-${p.id}" min="1" value="1" />
          <button class="btn btn-outline-primary mt-auto" onclick='agregarAlCarrito(${JSON.stringify(p)}, ${p.id})'>Agregar al carrito</button>
        </div>
      </div>
    `;

    contenedor.appendChild(col);
  });
}

document.getElementById('formProducto').addEventListener('submit', async (e) => {
  e.preventDefault();

  const nuevoProducto = {
    name: document.getElementById('name').value,
    category: document.getElementById('category').value,
    price: parseFloat(document.getElementById('price').value),
    description: document.getElementById('description').value,
    imageUrl: document.getElementById('imageUrl').value
  };

  try {
    const res = await fetch(apiUrl, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(nuevoProducto)
    });

    if (!res.ok) throw new Error(await res.text());

    alert('Producto agregado');
    e.target.reset();
    cargarProductos();
  } catch (err) {
    alert('Error al agregar producto: ' + err.message);
  }
});

function agregarAlCarrito(producto, id) {
  const cantidad = parseInt(document.getElementById(`cantidad-${id}`).value);
  if (cantidad <= 0 || isNaN(cantidad)) return alert("Cantidad inválida");

  const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
  const existente = carrito.find(p => p.id === producto.id);

  if (existente) {
    existente.cantidad += cantidad;
  } else {
    carrito.push({ ...producto, cantidad });
  }

  localStorage.setItem("carrito", JSON.stringify(carrito));
  actualizarContadorCarrito();
}

function actualizarContadorCarrito() {
  const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
  const total = carrito.reduce((sum, item) => sum + item.cantidad, 0);
  document.getElementById('contadorCarrito').textContent = total;
}

function aplicarFiltros() {
  const busqueda = document.getElementById('busquedaNavbar').value.toLowerCase();
  const precioMax = parseFloat(document.getElementById('precioMaximo').value) || Infinity;

  fetch(apiUrl)
    .then(res => res.json())
    .then(productos => {
      const filtrados = productos.filter(p => {
        const coincideNombre = p.name.toLowerCase().includes(busqueda);
        const coincidePrecio = p.price <= precioMax;
        return coincideNombre && coincidePrecio;
      });
      mostrarProductos(filtrados);
    })
    .catch(err => alert('Error al filtrar: ' + err.message));
}

// Inicialización
actualizarContadorCarrito();
cargarProductos();
