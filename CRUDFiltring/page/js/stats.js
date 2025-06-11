// Función para obtener el ID de usuario de la URL
function getUserIdFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('userId');
}

// Función para formatear números grandes
function formatNumber(num) {
    if (num >= 1000000) {
        return (num / 1000000).toFixed(1) + 'M';
    }
    if (num >= 1000) {
        return (num / 1000).toFixed(1) + 'K';
    }
    return num.toString();
}

// Función para animar números con efecto suave
function animateValue(element, start, end, duration) {
    const range = end - start;
    const increment = range / (duration / 16);
    let current = start;
    
    const timer = setInterval(() => {
        current += increment;
        if ((increment > 0 && current >= end) || (increment < 0 && current <= end)) {
            clearInterval(timer);
            element.textContent = formatNumber(end);
        } else {
            element.textContent = formatNumber(Math.floor(current));
        }
    }, 16);
}

// Función para mostrar mensajes de error
function showError(message) {
    const container = document.querySelector('.stats-container');
    const errorDiv = document.createElement('div');
    errorDiv.className = 'error-message';
    errorDiv.style.cssText = `
        background-color: #fee;
        color: #c00;
        padding: 1rem;
        border-radius: 8px;
        margin: 1rem 0;
        text-align: center;
        animation: fadeIn 0.3s ease-out;
    `;
    errorDiv.textContent = message;
    container.prepend(errorDiv);

    setTimeout(() => {
        errorDiv.style.animation = 'fadeOut 0.3s ease-out';
        setTimeout(() => errorDiv.remove(), 300);
    }, 5000);
}

// Función para actualizar el avatar del usuario
function updateUserAvatar(nombre, fotoPrincipal) {
    const avatar = document.getElementById('userAvatar');
    const avatarImg = document.getElementById('userAvatarImg');
    const avatarInitial = document.getElementById('userAvatarInitial');
    
    if (fotoPrincipal) {
        // Si hay foto, mostrarla
        avatarImg.src = fotoPrincipal;
        avatarImg.style.display = 'block';
        avatarInitial.style.display = 'none';
        avatar.style.background = 'none';
    } else {
        // Si no hay foto, mostrar inicial con color de fondo
        avatarImg.style.display = 'none';
        avatarInitial.style.display = 'flex';
        avatarInitial.textContent = nombre.charAt(0);
        
        const colors = [
            '#3498db', '#e74c3c', '#2ecc71', '#f1c40f', 
            '#9b59b6', '#1abc9c', '#e67e22', '#34495e'
        ];
        const colorIndex = nombre.charCodeAt(0) % colors.length;
        avatar.style.background = `linear-gradient(135deg, ${colors[colorIndex]}, ${colors[(colorIndex + 1) % colors.length]})`;
    }
}

// Función para verificar la sesión
async function checkSession() {
    try {
        const response = await fetch('./api/check_session.php', {
            method: 'GET',
            credentials: 'include'
        });

        if (!response.ok) {
            throw new Error('Error en la verificación de sesión');
        }

        const data = await response.json();
        
        if (!data.authenticated) {
            // Guardar la URL actual para redirigir después del login
            const currentPath = window.location.pathname + window.location.search;
            sessionStorage.setItem('redirectAfterLogin', currentPath);
            window.location.href = 'login.html';
            return false;
        }

        return data;
    } catch (error) {
        console.error('Error checking session:', error);
        window.location.href = 'login.html';
        return false;
    }
}

// Función principal para cargar las estadísticas
async function loadStats() {
    const sessionData = await checkSession();
    if (!sessionData) return;

    const userId = getUserIdFromUrl();
    if (!userId) {
        // Si no hay userId en la URL, usar el de la sesión
        if (sessionData.user && sessionData.user.id) {
            window.location.href = `stats.html?userId=${sessionData.user.id}`;
            return;
        }
        showError('ID de usuario no proporcionado');
        return;
    }

    // Mostrar el contenedor de estadísticas con estado de carga
    const statsContainer = document.querySelector('.stats-container');
    statsContainer.style.display = 'block';
    statsContainer.classList.add('loading');

    try {
        const response = await fetch(`api/stats.php?userId=${userId}`, {
            credentials: 'include'
        });
        
        if (response.status === 401) {
            const currentPath = window.location.pathname + window.location.search;
            sessionStorage.setItem('redirectAfterLogin', currentPath);
            window.location.href = 'login.html';
            return;
        }

        if (response.status === 403) {
            showError('No tienes permiso para ver estas estadísticas');
            return;
        }

        const data = await response.json();

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        if (data.success) {
            const usuario = data.data.usuario;
            if (usuario) {
                document.getElementById('userName').textContent = `${usuario.Nombre} ${usuario.Apellido}`;
                document.getElementById('userDetails').textContent = `@${usuario.User} | ${usuario.Genero}`;
                updateUserAvatar(usuario.Nombre, usuario.FotoPrincipal);
            }

            const stats = {
                'totalMatches': data.data.total_matches || 0,
                'activeMatches': data.data.matches_activos || 0,
                'sentMessages': data.data.mensajes_enviados || 0,
                'receivedMessages': data.data.mensajes_recibidos || 0
            };

            Object.entries(stats).forEach(([id, value], index) => {
                setTimeout(() => {
                    const element = document.getElementById(id);
                    if (element) {
                        animateValue(element, 0, value, 1500);
                    }
                }, index * 200);
            });
        } else {
            showError(data.message || 'Error al cargar las estadísticas');
        }
    } catch (error) {
        console.error('Error:', error);
        showError('Error al cargar las estadísticas');
    } finally {
        // Remover el estado de carga
        statsContainer.classList.remove('loading');
    }
}

// Función para cerrar sesión
async function logout() {
    try {
        const response = await fetch('./api/logout.php', {
            credentials: 'include'
        });
        if (!response.ok) {
            throw new Error('Error al cerrar sesión');
        }
    } catch (error) {
        console.error('Error during logout:', error);
    } finally {
        // Redirigir al login independientemente del resultado
        window.location.href = 'login.html';
    }
}

// Iniciar la carga cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', () => {
    const logoutButton = document.getElementById('logout');
    if (logoutButton) {
        logoutButton.addEventListener('click', logout);
    }
    loadStats();
}); 