# CRUDFiltring - Sistema de Gestión de Usuarios y Matches

## Descripción
CRUDFiltring es una aplicación de escritorio desarrollada en C# que proporciona un sistema completo para la gestión de usuarios, matches y mensajería. La aplicación está construida utilizando Windows Forms y .NET 6.0, con un backend web en PHP y MySQL.

## Características Principales
- Sistema de autenticación de usuarios
- Gestión de usuarios (CRUD)
- Sistema de matches y conexiones
- Sistema de mensajería integrado
- Interfaz de usuario moderna y responsiva
- Gestión de mensajes recibidos y respuestas
- Panel web para gestión de perfiles y estadísticas

## Requisitos del Sistema
### Aplicación de Escritorio
- Windows 10 o superior
- .NET 6.0 Runtime
- MySQL Server

### Servidor Web
- XAMPP (Apache + MySQL + PHP)
- PHP 8.2 o superior
- MySQL 10.4 o superior
- Apache 2.4 o superior

## Dependencias y Requisitos de Instalación

### 1. Aplicación de Escritorio (.NET)
- .NET 6.0 SDK (https://dotnet.microsoft.com/download/dotnet/6.0)
- Visual Studio 2022 o Visual Studio Code con extensiones C#
- Paquetes NuGet:
  ```
  MySql.Data (v9.2.0)
  Newtonsoft.Json (v13.0.3)
  YamlDotNet (v16.3.0)
  ```

### 2. Servidor Web (XAMPP)
- XAMPP (https://www.apachefriends.org/download.html)
  - Apache 2.4
  - MySQL 10.4
  - PHP 8.2
  - phpMyAdmin

### 3. Extensiones PHP Requeridas
- mysqli
- pdo_mysql
- json
- mbstring
- gd
- fileinfo
- curl

### 4. Base de Datos
- MySQL Server 10.4 o superior
- Base de datos: `filtring`
- Usuario por defecto:
  - Username: admin
  - Password: administrador

### 5. Navegadores Web Soportados
- Google Chrome (última versión)
- Mozilla Firefox (última versión)
- Microsoft Edge (última versión)

### 6. Requisitos del Sistema
#### Windows
- Windows 10 o superior
- 4GB RAM mínimo
- 1GB espacio en disco
- Resolución mínima: 1366x768

#### Servidor
- 2GB RAM mínimo
- 2GB espacio en disco
- Conexión a internet para actualizaciones

### 7. Herramientas de Desarrollo (Opcional)
- Git para control de versiones
- Postman para pruebas de API
- MySQL Workbench para gestión de base de datos

### 8. Configuración de PHP (php.ini)
```ini
memory_limit = 256M
upload_max_filesize = 10M
post_max_size = 10M
max_execution_time = 300
max_input_time = 300
```

### 9. Permisos de Directorios
- Directorio `uploads/`: 755 (drwxr-xr-x)
- Archivos PHP: 644 (-rw-r--r--)
- Archivos de configuración: 600 (-rw-------)

### 10. Variables de Entorno
```env
DB_HOST=localhost
DB_NAME=filtring
DB_USER=root
DB_PASS=
APP_URL=http://localhost/filtring
```

## Estructura del Proyecto
```
CRUDFiltring/
├── Forms/
│   ├── LogIn.cs                 # Formulario de inicio de sesión
│   ├── MainForm.cs             # Formulario principal
│   ├── UsersForm.cs            # Gestión de usuarios
│   ├── MatchesForm.cs          # Gestión de matches
│   ├── MessageForm.cs          # Sistema de mensajería
│   ├── PendingMatchesForm.cs   # Gestión de matches pendientes
│   └── ReceivedMessagesForm.cs # Mensajes recibidos
├── Components/
│   ├── ChatConnection.cs       # Lógica de conexión para chat
│   └── FormStyles.cs           # Estilos de formularios
└── page/                       # Panel Web
    ├── api/                    # Endpoints de la API
    ├── css/                    # Estilos CSS
    ├── js/                     # Scripts JavaScript
    ├── uploads/                # Archivos subidos
    ├── index.html             # Página principal
    ├── login.html             # Página de inicio de sesión
    ├── register.html          # Página de registro
    ├── profile.html           # Página de perfil
    ├── stats.html             # Página de estadísticas
    └── filtring.sql           # Estructura de la base de datos
```

## Configuración del Servidor Local
1. Instalar XAMPP
2. Iniciar los servicios de Apache y MySQL desde el panel de control de XAMPP
3. Copiar la carpeta `page` al directorio `htdocs` de XAMPP
4. Importar el archivo `filtring.sql` en phpMyAdmin
5. Configurar la conexión a la base de datos en la aplicación de escritorio

## Estructura de la Base de Datos
La base de datos `filtring` incluye las siguientes tablas:
- `usuario`: Información de usuarios
- `perfil`: Perfiles de usuario
- `fotosperfil`: Fotos de perfil
- `matches`: Conexiones entre usuarios
- `mensaje`: Sistema de mensajería

## Funcionalidades Principales

### Sistema de Autenticación
- Login seguro de usuarios
- Gestión de sesiones
- Registro de nuevos usuarios

### Gestión de Usuarios
- Crear nuevos usuarios
- Editar información de usuarios
- Eliminar usuarios
- Visualizar lista de usuarios
- Gestión de perfiles y fotos

### Sistema de Matches
- Crear nuevos matches
- Gestionar matches pendientes
- Visualizar matches activos
- Sistema de conexión entre usuarios

### Sistema de Mensajería
- Enviar mensajes
- Recibir mensajes
- Responder mensajes
- Historial de conversaciones

### Panel Web
- Gestión de perfiles
- Visualización de estadísticas
- Sistema de registro y login
- Interfaz responsiva

## Configuración
1. Asegúrese de tener instalado .NET 6.0 SDK
2. Clone el repositorio
3. Restaure los paquetes NuGet
4. Configure XAMPP y la base de datos MySQL
5. Importe el archivo SQL en phpMyAdmin
6. Configure la conexión a la base de datos en la aplicación
7. Compile y ejecute la aplicación

## Desarrollo
El proyecto está desarrollado utilizando:
- C# y Windows Forms para la aplicación de escritorio
- PHP y MySQL para el backend web
- HTML, CSS y JavaScript para el panel web
- Arquitectura en capas para mejor mantenibilidad

## Licencia
Este proyecto es privado y su uso está restringido a los propósitos autorizados.

## Soporte
Para soporte técnico o consultas, por favor contacte al equipo de desarrollo. 