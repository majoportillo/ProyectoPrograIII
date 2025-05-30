const API_URL = 'http://localhost:5234/api/productos';

function logout() {
    localStorage.removeItem('token');
    window.location.href = 'login.html';
}

async function cargarProductos() {
    const token = localStorage.getItem('token');
    if (!token) {
        alert('Debes iniciar sesión');
        window.location.href = 'login.html';
        return;
    }

    try {
        const res = await fetch(API_URL, {
            headers: { Authorization: `Bearer ${token}` }
        });

        if (!res.ok) throw new Error('No se pudieron cargar los productos');

        const productos = await res.json();
        const contenedor = document.getElementById('productos');

        contenedor.innerHTML = productos.map(p => `
      <div class="card p-3 mb-2">
        <strong>${p.nombre}</strong> - Q${p.precio} - Stock: ${p.stock}
      </div>
    `).join('');
    } catch (err) {
        console.error(err);
        alert('Error al cargar productos');
    }
}

async function crearProducto(event) {
    event.preventDefault();

    const token = localStorage.getItem('token');
    const nombre = document.getElementById('nombre').value;
    const precio = parseFloat(document.getElementById('precio').value);
    const stock = parseInt(document.getElementById('stock').value);
    const categoria = document.getElementById('categoria').value;

    try {
        const res = await fetch(API_URL, {
            method: 'POST',
            headers: {
                Authorization: `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ nombre, precio, stock, categoria })
        });

        if (!res.ok) throw new Error('Error al crear producto');

        alert('Producto agregado con éxito');
        cargarProductos();
    } catch (err) {
        console.error(err);
        alert('No se pudo agregar el producto');
    }
}

window.onload = cargarProductos;
