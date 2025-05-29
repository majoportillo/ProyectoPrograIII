document.addEventListener('DOMContentLoaded', () => {
  fetch('https://localhost:5001/api/productos') // Asegúrate de usar la URL real de tu API
    .then(response => {
      if (!response.ok) throw new Error('Error al obtener los productos');
      return response.json();
    })
    .then(data => {
      const lista = document.getElementById('lista-productos');
      data.forEach(producto => {
        const li = document.createElement('li');
        li.textContent = `${producto.nombre} - Q${producto.precio}`;
        lista.appendChild(li);
      });
    })
    .catch(error => {
      console.error('Error:', error);
    });
});
