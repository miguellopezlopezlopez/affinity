// Para que las etiquetas de los inputs funcionen correctamente
document.addEventListener('DOMContentLoaded', function() {
    // Inicializar los labels para los inputs
    document.querySelectorAll('.form-group input').forEach(input => {
        if (input.value !== '') {
            input.labels[0].classList.add('active');
        }
        
        input.addEventListener('focus', () => {
            input.labels[0].classList.add('active');
        });
        
        input.addEventListener('blur', () => {
            if (input.value === '') {
                input.labels[0].classList.remove('active');
            }
        });
    });

    // Inicializar Google Places Autocomplete
    const locationInput = document.getElementById('location');
    if (locationInput) {
        /* Comentando temporalmente la funcionalidad de Google Places
        const autocomplete = new google.maps.places.Autocomplete(locationInput, {
            types: ['(cities)'], // Restringir a solo ciudades
            fields: ['formatted_address', 'geometry']
        });

        // Manejar la selección de lugar
        autocomplete.addListener('place_changed', function() {
            const place = autocomplete.getPlace();
            if (!place.geometry) {
                showError('location', 'Por favor, selecciona una ubicación válida de la lista');
                return;
            }
            // La ubicación es válida
            locationInput.dataset.lat = place.geometry.location.lat();
            locationInput.dataset.lng = place.geometry.location.lng();
            clearError('location');
        });
        */

        // Prevenir envío del formulario al presionar enter en el campo de ubicación
        locationInput.addEventListener('keydown', function(e) {
            if (e.key === 'Enter') {
                e.preventDefault();
            }
        });
    }

    // Google Sign In handler
    const googleButton = document.querySelector('.google-sign-in');
    if (googleButton) {
        googleButton.addEventListener('click', function() {
            // Aquí iría la lógica de autenticación con Google
            console.log('Iniciando autenticación con Google...');
            alert('Esta función requiere configurar Google OAuth. Por favor, implementa la autenticación con Google según tus necesidades.');
        });
    }

    // Manejar la carga de foto de perfil
    const photoInput = document.getElementById('profilePhoto');
    const photoPreview = document.getElementById('photoPreview');
    const photoContainer = document.querySelector('.photo-preview-container');
    const removePhotoBtn = document.getElementById('removePhoto');
    const photoError = document.querySelector('.photo-error');

    if (photoInput && photoPreview) {
        // Función para mostrar error de foto
        function showPhotoError(message) {
            photoError.textContent = message;
            photoError.style.display = 'block';
            setTimeout(() => {
                photoError.style.display = 'none';
            }, 3000);
        }

        // Función para resetear la foto
        function resetPhoto() {
            photoInput.value = '';
            photoPreview.style.display = 'none';
            photoPreview.src = '#';
            photoContainer.classList.remove('has-image');
            removePhotoBtn.style.display = 'none';
            photoError.style.display = 'none';
        }

        // Evento para eliminar la foto
        removePhotoBtn.addEventListener('click', function(e) {
            e.stopPropagation(); // Evitar que se abra el selector de archivo
            resetPhoto();
        });

        photoInput.addEventListener('change', function(e) {
            const file = e.target.files[0];
            if (file) {
                // Validar tipo de archivo
                const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
                if (!allowedTypes.includes(file.type)) {
                    showPhotoError('Por favor, selecciona una imagen en formato JPG, PNG o GIF.');
                    resetPhoto();
                    return;
                }

                // Validar tamaño (5MB máximo)
                if (file.size > 5 * 1024 * 1024) {
                    showPhotoError('La imagen es demasiado grande. Por favor, selecciona una imagen menor a 5MB.');
                    resetPhoto();
                    return;
                }

                const reader = new FileReader();
                reader.onload = function(e) {
                    photoPreview.style.display = 'block';
                    photoPreview.src = e.target.result;
                    photoContainer.classList.add('has-image');
                    removePhotoBtn.style.display = 'block';
                };
                reader.onerror = function() {
                    showPhotoError('Error al leer la imagen. Por favor, intenta con otra imagen.');
                    resetPhoto();
                };
                reader.readAsDataURL(file);
            }
        });

        // Inicializar estado de la imagen
        resetPhoto();
    }
});

// Validar que las contraseñas coincidan
function validatePasswords() {
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    
    return password === confirmPassword;
}

// Convertir imagen a base64
async function getBase64Image(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

// Función para mostrar mensaje de error
function showError(field, message) {
    const formGroup = document.getElementById(field).closest('.form-group');
    formGroup.classList.add('has-error');
    const errorText = formGroup.querySelector('.error-text');
    if (errorText) {
        errorText.textContent = message;
    }
}

// Función para limpiar errores
function clearErrors() {
    document.querySelectorAll('.form-group').forEach(group => {
        group.classList.remove('has-error');
    });
    document.querySelectorAll('.error-text').forEach(error => {
        error.textContent = '';
    });
    const errorMessage = document.querySelector('.form-error-message');
    errorMessage.classList.remove('show');
    errorMessage.textContent = '';
}

// Función para mostrar mensaje de error general
function showGeneralError(message) {
    const errorMessage = document.querySelector('.form-error-message');
    errorMessage.textContent = message;
    errorMessage.classList.add('show');
}

// Función para mostrar mensaje de éxito
function showSuccessMessage(message) {
    const successMessage = document.querySelector('.form-success-message');
    successMessage.textContent = message;
    successMessage.classList.add('show');
}

// Simular el proceso de registro
async function simulateRegister() {
    const form = document.querySelector('.register-form');
    form.classList.add('form-submitted');
    clearErrors();

    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;
    const username = document.getElementById('username').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    const termsCheck = document.getElementById('termsCheck').checked;
    const gender = document.querySelector('input[name="gender"]:checked')?.value;
    const photoFile = document.getElementById('profilePhoto').files[0];
    const location = document.getElementById('location').value;
    
    // Validar formulario
    let hasErrors = false;
    
    if (!firstName) {
        showError('firstName', 'Por favor, introduce tu nombre');
        hasErrors = true;
    }
    
    if (!lastName) {
        showError('lastName', 'Por favor, introduce tu apellido');
        hasErrors = true;
    }
    
    if (!username) {
        showError('username', 'Por favor, elige un nombre de usuario');
        hasErrors = true;
    }
    
    if (!email) {
        showError('email', 'Por favor, introduce tu correo electrónico');
        hasErrors = true;
    } else {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            showError('email', 'Por favor, introduce un correo electrónico válido');
            hasErrors = true;
        }
    }
    
    if (!password) {
        showError('password', 'Por favor, crea una contraseña');
        hasErrors = true;
    } else if (password.length < 8) {
        showError('password', 'La contraseña debe tener al menos 8 caracteres');
        hasErrors = true;
    }
    
    if (!confirmPassword) {
        showError('confirmPassword', 'Por favor, confirma tu contraseña');
        hasErrors = true;
    } else if (password !== confirmPassword) {
        showError('confirmPassword', 'Las contraseñas no coinciden');
        hasErrors = true;
    }
    
    if (!gender) {
        showGeneralError('Por favor, selecciona tu género');
        hasErrors = true;
    }

    if (!location.trim()) {
        showError('location', 'Por favor, ingresa tu ubicación');
        hasErrors = true;
    }
    
    if (!termsCheck) {
        showGeneralError('Debes aceptar los términos y condiciones para continuar');
        hasErrors = true;
    }

    if (hasErrors) {
        return;
    }

    try {
        // Registrar usuario
        const registerResponse = await fetch('./api/register.php', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                user: username,
                password: password,
                nombre: firstName,
                apellido: lastName,
                genero: gender === 'male' ? 'Masculino' : (gender === 'female' ? 'Femenino' : 'Otro'),
                email: email,
                ubicacion: location
            })
        });

        const registerData = await registerResponse.json();

        if (!registerData.success) {
            throw new Error(registerData.message || 'Error en el registro');
        }

        // Si hay una foto seleccionada, subirla
        if (photoFile) {
            const formData = new FormData();
            formData.append('foto', photoFile);
            formData.append('tipo', 'principal');
            formData.append('userId', registerData.userId);

            const photoResponse = await fetch('./api/upload_photo.php', {
                method: 'POST',
                body: formData
            });

            const photoData = await photoResponse.json();
            if (!photoData.success) {
                console.error('Error al subir la foto:', photoData.message);
            }
        }

        showSuccessMessage('¡Registro exitoso! Redirigiendo al inicio de sesión...');
        setTimeout(() => {
            window.location.href = 'login.html';
        }, 2000);

    } catch (error) {
        console.error('Error:', error);
        showGeneralError('Error en el registro: ' + error.message);
    }
}

function toggleModal() {
    const modal = document.getElementById('termsModal');
    modal.style.display = modal.style.display === 'none' ? 'block' : 'none';
}

// Testimoniales rotativos
function initTestimonials() {
    const testimonials = document.querySelectorAll('.testimonial');
    let currentIndex = 0;
    let isAnimating = false;
    
    function showNextTestimonial() {
        if (isAnimating) return;
        isAnimating = true;
        
        const currentTestimonial = testimonials[currentIndex];
        const nextIndex = (currentIndex + 1) % testimonials.length;
        const nextTestimonial = testimonials[nextIndex];
        
        // Configurar el testimonial entrante
        nextTestimonial.style.transform = 'translateX(50px)';
        nextTestimonial.style.opacity = '0';
        nextTestimonial.style.visibility = 'visible';
        
        // Forzar un reflow para asegurar que las transiciones funcionen
        nextTestimonial.offsetHeight;
        
        // Animar el testimonial saliente
        currentTestimonial.style.transform = 'translateX(-50px)';
        currentTestimonial.style.opacity = '0';
        
        // Animar el testimonial entrante
        nextTestimonial.style.transform = 'translateX(0)';
        nextTestimonial.style.opacity = '1';
        
        // Limpiar después de la animación
        setTimeout(() => {
            currentTestimonial.style.visibility = 'hidden';
            isAnimating = false;
        }, 600);
        
        currentIndex = nextIndex;
    }
    
    // Rotación automática
    setInterval(showNextTestimonial, 5000);
    
    // Mostrar el primer testimonial
    testimonials[0].style.visibility = 'visible';
    testimonials[0].style.opacity = '1';
    testimonials[0].style.transform = 'translateX(0)';
}

// Inicializar los testimoniales cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', initTestimonials);

// Función para limpiar un error específico
function clearError(field) {
    const formGroup = document.getElementById(field).closest('.form-group');
    if (formGroup) {
        formGroup.classList.remove('has-error');
        const errorText = formGroup.querySelector('.error-text');
        if (errorText) {
            errorText.textContent = '';
        }
    }
} 