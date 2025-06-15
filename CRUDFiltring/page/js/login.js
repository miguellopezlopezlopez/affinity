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

// Función para mostrar mensajes de éxito
function showSuccess(message) {
    const successMessage = document.getElementById('successMessage');
    if (successMessage) {
        successMessage.textContent = message;
        successMessage.style.display = 'block';
    }
}

// Función para ocultar mensajes
function hideMessages() {
    const errorMessage = document.getElementById('errorMessage');
    const successMessage = document.getElementById('successMessage');
    
    if (errorMessage) errorMessage.style.display = 'none';
    if (successMessage) successMessage.style.display = 'none';
}

// Función para validar el formulario
function validateForm(email, password) {
    if (!email || !password) {
        showError('Por favor, completa todos los campos');
        return false;
    }
    
    // Validación básica de email
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email) && !email.includes('@')) {
        // Si no es email válido, permitir username
        if (email.length < 3) {
            showError('El usuario debe tener al menos 3 caracteres o usa un email válido');
            return false;
        }
    }
    
    return true;
}

// Función mejorada para generar URLs del API
function getApiUrls(endpoint) {
    const currentLocation = window.location;
    const protocol = currentLocation.protocol;
    const hostname = currentLocation.hostname;
    
    const baseUrls = [
        `${protocol}//${hostname}/page/api/${endpoint}`,
        `${protocol}//${hostname}/api/${endpoint}`,
        `./api/${endpoint}`,
        `api/${endpoint}`
    ];
    
    if (hostname === 'localhost' || hostname === '127.0.0.1') {
        baseUrls.push(
            `${protocol}//localhost/page/api/${endpoint}`,
            `${protocol}//127.0.0.1/page/api/${endpoint}`
        );
    }
    
    console.log('URLs a probar:', baseUrls);
    return baseUrls;
}

// Función principal para manejar el inicio de sesión
async function handleLogin(event) {
    if (event) {
        event.preventDefault();
    }
    
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value;

    hideMessages();

    if (!validateForm(email, password)) {
        return false;
    }

    const loginButton = document.querySelector('.register-button');
    const originalText = loginButton.textContent;
    
    try {
        loginButton.textContent = 'Conectando...';
        loginButton.disabled = true;
        
        console.log('Iniciando proceso de login...');
        
        const apiUrls = getApiUrls('login.php');
        
        for (const url of apiUrls) {
            try {
                console.log(`Intentando login con: ${url}`);
                
                loginButton.textContent = `Probando conexión...`;
                
                const controller = new AbortController();
                const timeoutId = setTimeout(() => controller.abort(), 10000);
                
                const requestData = { 
                    email: email, 
                    password: password 
                };
                
                const response = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    credentials: 'include',
                    body: JSON.stringify(requestData),
                    signal: controller.signal
                });

                clearTimeout(timeoutId);
                console.log(`Respuesta de ${url}:`, response.status);
                
                if (response.ok) {
                    const responseText = await response.text();
                    console.log('Respuesta raw:', responseText);
                    
                    let data;
                    try {
                        data = JSON.parse(responseText);
                        console.log('Datos parseados:', data);
                    } catch (parseError) {
                        console.error('Error parsing JSON:', parseError);
                        throw new Error(`Respuesta inválida del servidor`);
                    }
                    
                    if (data.success) {
                        showSuccess('¡Inicio de sesión exitoso! Redirigiendo...');
                        
                        // Guardar userId en sessionStorage
                        sessionStorage.setItem('userId', data.user.id);
                        sessionStorage.removeItem('redirectAfterLogin');
                        
                        console.log('Login exitoso, redirigiendo...');
                        
                        // Redirigir inmediatamente sin timeouts innecesarios
                        setTimeout(() => {
                            const profileUrl = `profile.html?userId=${data.user.id}`;
                            console.log('Redirigiendo a:', profileUrl);
                            window.location.href = profileUrl;
                        }, 1000);
                        
                        return;
                    } else {
                        showError(data.message || 'Credenciales incorrectas');
                        return;
                    }
                } else {
                    const errorText = await response.text();
                    console.error(`Error HTTP ${response.status}:`, errorText);
                    
                    if (response.status >= 500) {
                        continue; // Probar siguiente URL si es error de servidor
                    }
                    
                    throw new Error(`Error ${response.status}: ${errorText.substring(0, 100)}`);
                }
                
            } catch (error) {
                console.error(`Error con URL ${url}:`, error);
                
                if (error.name === 'AbortError' || error.name === 'TypeError') {
                    continue; // Probar siguiente URL
                }
                
                throw error;
            }
        }
        
        // Si ninguna URL funcionó
        throw new Error(`❌ No se pudo conectar con el servidor PHP.

🔧 Verificar:
1. XAMPP está ejecutándose (Apache activo)
2. El archivo api/login.php existe
3. La base de datos 'filtring' está creada
4. Revisar logs de Apache/PHP`);
        
    } catch (error) {
        console.error('Error en handleLogin:', error);
        showError(error.message);
    } finally {
        loginButton.disabled = false;
        loginButton.textContent = originalText;
    }
    
    return false;
}

// **ELIMINADA LA FUNCIÓN checkIfAlreadyLoggedIn() que causaba el bucle**
// **SIMPLIFICADA la verificación de sesión**

// Función simple para verificar si hay sesión válida
async function checkCurrentSession() {
    try {
        const checkUrls = getApiUrls('check_session.php');
        
        for (const url of checkUrls) {
            try {
                const controller = new AbortController();
                const timeoutId = setTimeout(() => controller.abort(), 3000);
                
                const response = await fetch(url, {
                    method: 'GET',
                    credentials: 'include',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    signal: controller.signal
                });
                
                clearTimeout(timeoutId);
                
                if (response.ok) {
                    const data = await response.json();
                    console.log('Verificación de sesión:', data);
                    
                    if (data.authenticated) {
                        // Solo redirigir si estamos en la página de login
                        if (window.location.pathname.includes('login.html') || 
                            window.location.pathname.includes('index.html')) {
                            
                            console.log('Usuario ya autenticado, redirigiendo...');
                            const profileUrl = `profile.html?userId=${data.user.id}`;
                            window.location.href = profileUrl;
                        }
                        return true;
                    }
                    return false;
                }
            } catch (err) {
                console.log('Error verificando sesión:', err.message);
                continue;
            }
        }
        return false;
    } catch (error) {
        console.log('Error general verificando sesión:', error.message);
        return false;
    }
}

// Limpiar datos locales (manteniendo solo lo necesario)
function clearLocalSession() {
    sessionStorage.removeItem('redirectAfterLogin');
    // No limpiar otros datos ya que ahora confiamos en la sesión del servidor
}

// Event listener principal - SIMPLIFICADO
// Event listener principal - SIN VERIFICACIÓN AUTOMÁTICA
document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM cargado, inicializando login...');
    
    // DISABLED: Verificación automática de sesión para evitar bucles
    // Solo manejar el formulario de login
    
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        console.log('Formulario de login encontrado');
        
        loginForm.addEventListener('submit', async function(e) {
            e.preventDefault();
            
            // Reset messages
            const errorMessage = document.getElementById('errorMessage');
            const successMessage = document.getElementById('successMessage');
            errorMessage.style.display = 'none';
            successMessage.style.display = 'none';
            
            const user = document.getElementById('email').value.trim();
            const password = document.getElementById('password').value;

            // Validación básica
            if (!user || !password) {
                errorMessage.textContent = 'Por favor, completa todos los campos';
                errorMessage.style.display = 'block';
                return;
            }

            const loginButton = document.querySelector('.register-button');
            const originalText = loginButton.textContent;
            
            try {
                loginButton.textContent = 'Conectando...';
                loginButton.disabled = true;

                const response = await fetch('api/login.php', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        user: user,
                        password: password
                    })
                });

                const data = await response.json();

                if (data.success) {
                    successMessage.textContent = '¡Inicio de sesión exitoso! Redirigiendo...';
                    successMessage.style.display = 'block';
                    
                    // Guardar datos del usuario
                    localStorage.setItem('user', JSON.stringify(data.user));
                    
                    // Redirigir al perfil
                    setTimeout(() => {
                        window.location.href = 'profile.html';
                    }, 1000);
                } else {
                    errorMessage.textContent = data.message || 'Usuario o contraseña incorrectos';
                    errorMessage.style.display = 'block';
                    loginForm.classList.add('shake');
                    setTimeout(() => loginForm.classList.remove('shake'), 500);
                }
            } catch (error) {
                console.error('Error:', error);
                errorMessage.textContent = 'Error al conectar con el servidor';
                errorMessage.style.display = 'block';
            } finally {
                loginButton.disabled = false;
                loginButton.textContent = originalText;
            }
        });
        
        // Manejar Enter en los campos
        const inputs = loginForm.querySelectorAll('input');
        inputs.forEach((input) => {
            input.addEventListener('keypress', function(e) {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    handleLogin(e);
                }
            });
        });
    }
    
    // Mostrar mensaje si hay redirección pendiente
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
        const form = document.querySelector('.register-form');
        if (form) {
            form.prepend(message);
        }
    }
});

// Funciones del modal de recuperación de contraseña (mantenidas)
function showRecoverPasswordModal() {
    const modal = document.getElementById('recoverPasswordModal');
    if (modal) {
        modal.style.display = 'flex';
        showStep(1);
    }
}

function closeModal() {
    const modal = document.getElementById('recoverPasswordModal');
    if (modal) {
        modal.style.display = 'none';
        document.getElementById('recoveryEmail').value = '';
        const codeInputs = document.querySelectorAll('.verification-code-input input');
        codeInputs.forEach(input => input.value = '');
        document.getElementById('newPassword').value = '';
        document.getElementById('confirmNewPassword').value = '';
        hideModalMessages();
    }
}

function hideModalMessages() {
    for (let i = 1; i <= 3; i++) {
        const errorMsg = document.getElementById(`modalErrorMessage${i}`);
        const successMsg = document.getElementById(`modalSuccessMessage${i}`);
        if (errorMsg) errorMsg.style.display = 'none';
        if (successMsg) successMsg.style.display = 'none';
    }
}

function showModalError(step, message) {
    const errorElement = document.getElementById(`modalErrorMessage${step}`);
    if (errorElement) {
        errorElement.textContent = message;
        errorElement.style.display = 'block';
    }
}

function showModalSuccess(step, message) {
    const successElement = document.getElementById(`modalSuccessMessage${step}`);
    if (successElement) {
        successElement.textContent = message;
        successElement.style.display = 'block';
    }
}

function showStep(stepNumber) {
    document.querySelectorAll('.modal-steps').forEach(step => {
        step.classList.remove('active');
    });
    
    const currentStep = document.getElementById(`step${stepNumber}`);
    if (currentStep) {
        currentStep.classList.add('active');
    }
    
    hideModalMessages();
}

// Cerrar modal al hacer clic fuera
window.addEventListener('click', function(event) {
    const modal = document.getElementById('recoverPasswordModal');
    if (event.target === modal) {
        closeModal();
    }
});