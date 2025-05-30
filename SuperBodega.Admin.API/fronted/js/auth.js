async function login() {
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value.trim();
    const errorDiv = document.getElementById('error');

    if (!email || !password) {
        errorDiv.innerText = 'Todos los campos son obligatorios';
        return;
    }

    try {
        const response = await fetch('http://localhost:5234/api/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ UsuarioNombre: email, Password: password })
        });

        const result = await response.text();
        console.log('Respuesta del servidor:', result);

        if (!response.ok) throw new Error('Credenciales inválidas');

        const data = JSON.parse(result);
        localStorage.setItem('token', data);
        window.location.href = 'dashboard.html';
    } catch (error) {
        errorDiv.innerText = error.message;
    }
}
