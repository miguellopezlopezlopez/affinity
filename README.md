# Affinty

## Descripción
Affinty es una plataforma integral de emparejamiento y mensajería construida con C# Windows Forms y backend en PHP. La aplicación permite a los usuarios conectarse, emparejarse y comunicarse con otros usuarios según sus preferencias y perfiles.

## Tecnologías Utilizadas
- **Frontend Escritorio**: C# Windows Forms (.NET)
- **Backend**: PHP
- **Base de Datos**: MySQL
- **Interfaz Web**: HTML, CSS, JavaScript

## Características
- **Autenticación de Usuarios**
  - Sistema seguro de inicio de sesión y registro
  - Gestión de perfiles
  
- **Sistema de Emparejamiento**
  - Ver posibles coincidencias
  - Enviar y recibir solicitudes de emparejamiento
  - Aceptar o rechazar emparejamientos pendientes
  
- **Sistema de Mensajería**
  - Enviar y recibir mensajes
  - Responder a mensajes
  - Ver historial de mensajes
  
- **Gestión de Usuarios**
  - Actualización de perfiles
  - Búsqueda y filtrado de usuarios
  - Carga de fotos de perfil

## Estructura del Proyecto
```
├── CRUDFiltring/              # Directorio principal de la aplicación
│   ├── page/                  # Archivos relacionados con la web
│   │   ├── api/              # APIs de backend en PHP
│   │   ├── js/               # Archivos JavaScript
│   │   ├── css/              # Hojas de estilo
│   │   ├── uploads/          # Archivos subidos por usuarios
│   │   └── *.html            # Páginas de interfaz web
│   │
│   ├── Forms/                # Formularios de Windows
│   │   ├── LogIn.cs          # Formulario de inicio de sesión
│   │   ├── MainForm.cs       # Ventana principal de la aplicación
│   │   ├── MatchesForm.cs    # Gestión de emparejamientos
│   │   ├── MessageForm.cs    # Interfaz de mensajería
│   │   ├── UsersForm.cs      # Gestión de usuarios
│   │   ├── ChatConnection.cs  # Funcionalidad de chat
│   │   └── FormStyles.cs     # Utilidades de estilo de UI
│  
│      
```

## Instrucciones de Configuración

### Requisitos Previos
1. .NET Framework (para la aplicación Windows Forms)
2. PHP 7.4 o superior
3. Servidor MySQL
4. Servidor web (Apache/Nginx)

### Configuración de la Base de Datos
1. Importar el archivo `filtring.sql` desde el directorio `page` a tu servidor MySQL
2. Configurar la conexión a la base de datos en `page/api/config.php`

### Configuración de la Aplicación
1. Abrir la solución en Visual Studio
2. Restaurar paquetes NuGet si es necesario
3. Actualizar los endpoints de la API para que coincidan con la configuración de tu servidor
4. Compilar y ejecutar la aplicación

### Configuración del Servidor Web
1. Copiar el contenido del directorio `page` a tu servidor web
2. Configurar tu servidor web para servir archivos PHP
3. Asegurar que el directorio `uploads` tenga permisos de escritura

## Características de Seguridad
- Encriptación de contraseñas
- Gestión de sesiones
- Manejo seguro de carga de archivos
- Autenticación de API

## Contribución
1. Hacer un fork del repositorio
2. Crear una rama de características
3. Realizar los cambios
4. Hacer push a la rama
5. Crear un Pull Request

## Licencia
Este proyecto esta bajo la licencia MIT
