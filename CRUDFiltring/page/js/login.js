// Función para mostrar mensajes de error
function showError(message) {
    const errorMessage = document.getElementById('errorMessage');
    errorMessage.textContent = message;
    errorMessage.style.display = 'block';
    document.querySelector('.register-form').classList.add('shake');
    setTimeout(() => {
        document.querySelector('.register-form').classList.remove('shake');
    }, 500);
}

// Función para validar el formulario
function validateForm(email, password) {
    if (!email || !password) {
        showError('Por favor, completa todos los campos');
        return false;
    }
    return true;
}

// Función principal para manejar el inicio de sesión
async function handleLogin() {
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const errorMessage = document.getElementById('errorMessage');

    // Ocultar mensaje de error previo
    errorMessage.style.display = 'none';

    if (!validateForm(email, password)) {
        return;
    }

    const loginButton = document.querySelector('.register-button');
    const originalText = loginButton.textContent;
    loginButton.textContent = 'Iniciando sesión...';
    loginButton.disabled = true;

    try {
        const response = await fetch('./api/login.php', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify({ email, password })
        });

        const data = await response.json();

        if (data.success) {
            // Verificar si hay una redirección guardada que no sea la página de inicio
            const redirectPath = sessionStorage.getItem('redirectAfterLogin');
            if (redirectPath && !redirectPath.includes('index.html')) {
                sessionStorage.removeItem('redirectAfterLogin');
                window.location.href = redirectPath;
            } else {
                // Si no hay redirección específica o es la página de inicio,
                // redirigir al perfil del usuario
                window.location.href = `stats.html?userId=${data.user.id}`;
            }
        } else {
            showError(data.message || 'Error al iniciar sesión');
            loginButton.disabled = false;
            loginButton.textContent = originalText;
        }
    } catch (error) {
        console.error('Error:', error);
        showError('Error de conexión');
        loginButton.disabled = false;
        loginButton.textContent = originalText;
    }
}

// Permitir envío del formulario con Enter
document.getElementById('loginForm').addEventListener('keypress', function(e) {
    if (e.key === 'Enter') {
        e.preventDefault();
        handleLogin();
    }
});

// Verificar si hay una redirección pendiente al cargar la página
document.addEventListener('DOMContentLoaded', function() {
    const redirectPath = sessionStorage.getItem('redirectAfterLogin');
    if (redirectPath) {
        const message = document.createElement('div');
        message.className = 'info-message';
        message.style.cssText = `
            background-color: #e8f5e9;
            color: #2e7d32;
            padding: 1rem;
            border-radius: 8px;
            margin: 1rem 0;
            text-align: center;
        `;
        message.textContent = 'Por favor, inicia sesión para continuar';
        document.querySelector('.register-form').prepend(message);
    }
}); 