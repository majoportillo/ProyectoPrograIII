async function cargarProductos() {
    const token = localStorage.getItem('token');
    if (!token) {
        alert('Debes iniciar sesión');
        window.location.href = 'login.html';
        return;
    }

    try {
        const res = await fetch('http://localhost:5234/api/productos', {
            headers: { Authorization: `Bearer ${token}` }
        });

        if (!res.ok) throw new Error('Error al cargar productos');

        const productos = await res.json();
        const contenedor = document.getElementById('productos');
        contenedor.innerHTML = productos.map(p => `
      <div class="card p-3 mb-2">
        <strong>${p.nombre}</strong> - Q${p.precio} - Stock: ${p.stock}
      </div>
    `).join('');
    } catch (err) {
        alert(err.message);
    }
}

function logout() {
    localStorage.removeItem('token');
    window.location.href = 'login.html';
}

window.onload = cargarProductos;
