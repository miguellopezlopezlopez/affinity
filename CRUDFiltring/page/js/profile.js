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
    return urlParams.get('userId');
}

async function loadProfileData() {
    try {
        const userId = getUserIdFromUrl();
        const url = userId ? `/page/api/profile.php?userId=${userId}` : '/page/api/profile.php';

        // Cargar datos del perfil desde el servidor
        const response = await fetch(url);
        
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
        const formData = new FormData();
        formData.append('foto', file);
        formData.append('tipo', 'principal');

        const response = await fetch('/page/api/upload_photo.php', {
            method: 'POST',
            body: formData
        });

        if (response.status === 401) {
            window.location.href = '/page/login.html';
            return;
        }

        const data = await response.json();
        if (!data.success) {
            throw new Error(data.message || 'Error al subir la foto');
        }

        document.getElementById('mainPhotoPreview').src = data.url;
    } catch (error) {
        console.error('Error:', error);
        alert('Error al subir la foto: ' + error.message);
    }
}

async function handleAdditionalPhotosUpload(event) {
    const files = event.target.files;
    const maxFiles = 5;
    const currentPhotos = document.querySelectorAll('.gallery-item').length;

    if (currentPhotos + files.length > maxFiles) {
        alert(`Solo puedes tener un máximo de ${maxFiles} fotos en la galería`);
        return;
    }

    for (let file of files) {
        try {
            const formData = new FormData();
            formData.append('foto', file);
            formData.append('tipo', 'galeria');

            const response = await fetch('/page/api/upload_photo.php', {
                method: 'POST',
                body: formData
            });

            if (response.status === 401) {
                window.location.href = '/page/login.html';
                return;
            }

            const data = await response.json();
            if (!data.success) {
                throw new Error(data.message || 'Error al subir la foto');
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Error al subir la foto: ' + error.message);
        }
    }

    // Recargar la galería después de subir todas las fotos
    loadProfileData();
}

async function deletePhoto(photoId) {
    if (!confirm('¿Estás seguro de que quieres eliminar esta foto?')) return;

    try {
        const response = await fetch('/page/api/delete_photo.php', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ photoId })
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
        const data = {
            biografia: document.getElementById('biografia').value,
            intereses: document.getElementById('intereses').value,
            preferencias: document.getElementById('preferencias').value
        };

        const response = await fetch('/page/api/update_profile.php', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        });

        if (response.status === 401) {
            window.location.href = '/page/login.html';
            return;
        }

        const result = await response.json();
        if (!result.success) {
            throw new Error(result.message || 'Error al actualizar el perfil');
        }

        alert('Perfil actualizado correctamente');
    } catch (error) {
        console.error('Error:', error);
        alert('Error al guardar los cambios: ' + error.message);
    }
}

async function logout() {
    try {
        const response = await fetch('/page/api/logout.php');
        if (!response.ok) {
            throw new Error('Error al cerrar sesión');
        }
    } catch (error) {
        console.error('Error during logout:', error);
    } finally {
        window.location.href = '/page/login.html';
    }
} 