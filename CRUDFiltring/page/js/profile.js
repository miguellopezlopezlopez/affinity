document.addEventListener('DOMContentLoaded', function() {
    loadProfileData();

    // Manejar la foto de perfil principal
    document.getElementById('mainProfilePhoto').addEventListener('change', function(e) {
        handleMainPhotoUpload(e);
    });

    // Manejar fotos adicionales
    document.getElementById('additionalPhotos').addEventListener('change', function(e) {
        handleAdditionalPhotosUpload(e);
    });

    // Manejar envío del formulario
    document.getElementById('profileForm').addEventListener('submit', function(e) {
        e.preventDefault();
        saveProfileData();
    });

    // Manejar cierre de sesión
    document.getElementById('logout').addEventListener('click', function(e) {
        e.preventDefault();
        logout();
    });
});

function getUserIdFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    const urlUserId = urlParams.get('userId');
    // Si no hay userId en la URL, usar el de la sesión
    if (!urlUserId) {
        return sessionStorage.getItem('userId');
    }
    return urlUserId;
}

async function loadProfileData() {
    try {
        const userId = getUserIdFromUrl();
        const url = userId ? `/page/api/profile.php?userId=${userId}` : '/page/api/profile.php';

        // ✅ CORREGIDO: Agregado credentials: 'include'
        const response = await fetch(url, {
            credentials: 'include'
        });
        
        if (response.status === 401) {
            // No autenticado, redirigir al login
            window.location.href = '/page/login.html';
            return;
        }

        const data = await response.json();
        
        if (!data.success) {
            throw new Error(data.message || 'Error al cargar el perfil');
        }

        document.getElementById('userName').textContent = data.nombre + ' ' + data.apellido;
        document.getElementById('userLocation').textContent = data.ubicacion || 'Sin ubicación';
        
        // Solo mostrar y habilitar la edición si es el perfil propio
        const isOwnProfile = data.isOwnProfile;
        const editableElements = document.querySelectorAll('.editable');
        editableElements.forEach(el => {
            el.style.display = isOwnProfile ? 'block' : 'none';
        });

        if (isOwnProfile) {
            document.getElementById('biografia').value = data.biografia || '';
            document.getElementById('intereses').value = data.intereses || '';
            document.getElementById('preferencias').value = data.preferencias || '';
        } else {
            // Para perfiles de otros usuarios, mostrar la información en formato de solo lectura
            document.getElementById('biografia').textContent = data.biografia || 'Sin biografía';
            document.getElementById('intereses').textContent = data.intereses || 'Sin intereses';
            document.getElementById('preferencias').textContent = data.preferencias || 'Sin preferencias';
        }
        
        // Cargar foto principal
        if (data.foto_principal) {
            document.getElementById('mainPhotoPreview').src = data.foto_principal;
        }

        // Cargar galería de fotos
        if (data.fotos) {
            loadPhotoGallery(data.fotos, isOwnProfile);
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Error al cargar los datos del perfil: ' + error.message);
    }
}

function loadPhotoGallery(fotos, isOwnProfile) {
    const gallery = document.getElementById('photoGallery');
    gallery.innerHTML = '';

    fotos.forEach(foto => {
        const div = document.createElement('div');
        div.className = 'gallery-item';
        div.innerHTML = `
            <img src="${foto.url}" alt="Foto de galería">
            ${isOwnProfile ? `<button class="delete-photo" onclick="deletePhoto(${foto.id})">×</button>` : ''}
        `;
        gallery.appendChild(div);
    });
}

async function handleMainPhotoUpload(event) {
    const file = event.target.files[0];
    if (!file) return;

    try {
        console.log('Iniciando subida de foto principal...');
        console.log('Archivo seleccionado:', file.name);
        
        const userId = getUserIdFromUrl();
        console.log('userId from URL:', userId);
        
        if (!userId) {
            throw new Error('No se pudo obtener el ID del usuario');
        }
        
        const formData = new FormData();
        formData.append('foto', file);
        formData.append('tipo', 'principal');
        formData.append('userId', userId);

        console.log('FormData creado:', {
            foto: file.name,
            tipo: 'principal',
            userId: userId
        });

        // ✅ CORREGIDO: Agregado credentials: 'include'
        console.log('Enviando solicitud al servidor...');
        const response = await fetch('/page/api/upload_photo.php', {
            method: 'POST',
            body: formData,
            credentials: 'include'
        });

        console.log('Respuesta del servidor:', response.status);

        if (response.status === 401) {
            console.log('Error de autenticación');
            window.location.href = '/page/login.html';
            return;
        }

        const data = await response.json();
        console.log('Datos de respuesta:', data);

        if (!data.success) {
            throw new Error(data.message || 'Error al subir la foto');
        }

        // Buscar o crear el elemento de imagen
        let imgElement = document.getElementById('mainPhotoPreview');
        if (!imgElement) {
            console.log('Creando nuevo elemento de imagen...');
            imgElement = document.createElement('img');
            imgElement.id = 'mainPhotoPreview';
            imgElement.alt = 'Foto de perfil';
            const container = document.querySelector('.photo-preview-container');
            container.appendChild(imgElement);
        }
        
        // Actualizar la imagen
        imgElement.src = data.url;
        console.log('Imagen actualizada correctamente');

    } catch (error) {
        console.error('Error detallado:', error);
        alert('Error al subir la foto: ' + error.message);
    }
}

async function handleAdditionalPhotosUpload(event) {
    const files = event.target.files;
    const maxFiles = 5;
    const currentPhotos = document.querySelectorAll('.gallery-item').length;

    console.log('Iniciando subida de fotos adicionales...');
    console.log('Número de archivos seleccionados:', files.length);
    console.log('Fotos actuales en galería:', currentPhotos);

    if (currentPhotos + files.length > maxFiles) {
        alert(`Solo puedes tener un máximo de ${maxFiles} fotos en la galería`);
        return;
    }

    // Obtener el ID del usuario
    let userId = getUserIdFromUrl();
    
    // Si no hay ID en la URL, intentar obtenerlo de la sesión
    if (!userId) {
        userId = sessionStorage.getItem('userId');
    }

    // Si aún no hay ID, verificar la sesión
    if (!userId) {
        try {
            const sessionResponse = await fetch('./api/check_session.php', {
                credentials: 'include'
            });
            const sessionData = await sessionResponse.json();
            if (sessionData.authenticated && sessionData.user && sessionData.user.id) {
                userId = sessionData.user.id;
                sessionStorage.setItem('userId', userId);
            }
        } catch (error) {
            console.error('Error al verificar la sesión:', error);
        }
    }

    if (!userId) {
        alert('No se pudo obtener el ID del usuario. Por favor, inicia sesión nuevamente.');
        window.location.href = 'login.html';
        return;
    }

    for (let file of files) {
        try {
            console.log('Procesando archivo:', file.name);
            
            const formData = new FormData();
            formData.append('foto', file);
            formData.append('tipo', 'galeria');
            formData.append('userId', userId);

            console.log('FormData creado:', {
                foto: file.name,
                tipo: 'galeria',
                userId: userId
            });

            console.log('Enviando solicitud al servidor...');
            const response = await fetch('./api/upload_photo.php', {
                method: 'POST',
                body: formData,
                credentials: 'include'
            });

            console.log('Respuesta del servidor:', response.status);

            if (response.status === 401) {
                console.log('Error de autenticación');
                window.location.href = 'login.html';
                return;
            }

            const data = await response.json();
            console.log('Datos de respuesta:', data);

            if (!data.success) {
                throw new Error(data.message || 'Error al subir la foto');
            }
        } catch (error) {
            console.error('Error detallado:', error);
            alert('Error al subir la foto: ' + error.message);
        }
    }

    // Recargar la galería después de subir todas las fotos
    console.log('Recargando galería...');
    loadProfileData();
}

async function deletePhoto(photoId) {
    if (!confirm('¿Estás seguro de que quieres eliminar esta foto?')) return;

    try {
        // ✅ CORREGIDO: Agregado credentials: 'include'
        const response = await fetch('/page/api/delete_photo.php', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ photoId }),
            credentials: 'include'
        });

        if (response.status === 401) {
            window.location.href = '/page/login.html';
            return;
        }

        const data = await response.json();
        if (!data.success) {
            throw new Error(data.message || 'Error al eliminar la foto');
        }

        loadProfileData(); // Recargar la galería
    } catch (error) {
        console.error('Error:', error);
        alert('Error al eliminar la foto: ' + error.message);
    }
}

async function saveProfileData() {
    try {
        console.log('Iniciando guardado de datos del perfil...');
        
        const userId = getUserIdFromUrl();
        console.log('userId from URL:', userId);
        
        if (!userId) {
            throw new Error('No se pudo obtener el ID del usuario');
        }

        const data = {
            userId: userId,
            biografia: document.getElementById('biografia').value,
            intereses: document.getElementById('intereses').value,
            preferencias: document.getElementById('preferencias').value
        };

        console.log('Datos a enviar:', data);

        // ✅ CORREGIDO: Agregado credentials: 'include'
        console.log('Enviando solicitud al servidor...');
        const response = await fetch('/page/api/update_profile.php', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
            credentials: 'include'
        });

        console.log('Respuesta del servidor:', response.status);

        if (response.status === 401) {
            console.log('Error de autenticación');
            window.location.href = '/page/login.html';
            return;
        }

        const result = await response.json();
        console.log('Datos de respuesta:', result);

        if (!result.success) {
            throw new Error(result.message || 'Error al actualizar el perfil');
        }

        alert('Perfil actualizado correctamente');
    } catch (error) {
        console.error('Error detallado:', error);
        alert('Error al guardar los cambios: ' + error.message);
    }
}

async function logout() {
    try {
        // ✅ CORREGIDO: Agregado credentials: 'include'
        const response = await fetch('/page/api/logout.php', {
            credentials: 'include'
        });
        if (!response.ok) {
            throw new Error('Error al cerrar sesión');
        }
    } catch (error) {
        console.error('Error during logout:', error);
    } finally {
        window.location.href = '/page/login.html';
    }
}