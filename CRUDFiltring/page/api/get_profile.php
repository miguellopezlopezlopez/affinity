<?php
// Configuración de errores
error_reporting(E_ALL);
ini_set('display_errors', 0);

// Headers CORS y HTTP necesarios
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type, Accept');
header('Content-Type: application/json; charset=utf-8');

// Log de inicio
error_log("Iniciando get_profile.php");

// Manejar preflight requests
if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

// Verificar método GET
if ($_SERVER['REQUEST_METHOD'] !== 'GET') {
    error_log("Método no permitido: " . $_SERVER['REQUEST_METHOD']);
    http_response_code(405);
    echo json_encode([
        'success' => false,
        'message' => 'Método no permitido'
    ]);
    exit();
}

try {
    require_once 'config.php';

    // Validar parámetro de usuario
    if (!isset($_GET['user']) || empty($_GET['user'])) {
        error_log("Usuario no especificado en la solicitud");
        throw new Exception('Usuario no especificado');
    }

    $user = trim($_GET['user']);
    error_log("Buscando perfil para usuario: " . $user);

    // Verificar conexión
    if (!isset($conn) || $conn->connect_error) {
        error_log("Error de conexión: " . $conn->connect_error);
        throw new Exception("Error de conexión a la base de datos");
    }

    // Preparar y ejecutar consulta
    $stmt = $conn->prepare("SELECT ID, User, Email, Nombre, Apellido, Genero, Ubicacion, Foto FROM Usuario WHERE User = ?");
    if (!$stmt) {
        error_log("Error en prepare: " . $conn->error);
        throw new Exception("Error preparando la consulta");
    }

    $stmt->bind_param("s", $user);
    
    if (!$stmt->execute()) {
        error_log("Error en execute: " . $stmt->error);
        throw new Exception("Error ejecutando la consulta");
    }

    $result = $stmt->get_result();
    error_log("Número de resultados: " . $result->num_rows);

    if ($result->num_rows > 0) {
        $row = $result->fetch_assoc();
        error_log("Datos del usuario encontrado: " . json_encode($row));

        // Construir URL completa para la foto
        $foto = $row['Foto'];
        if (!empty($foto) && strpos($foto, 'http') !== 0) {
            $protocol = isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? 'https' : 'http';
            $host = $_SERVER['HTTP_HOST'];
            $foto = "$protocol://$host/page/" . $foto;
        }

        // Preparar respuesta
        $response = [
            'success' => true,
            'data' => [
                'id' => intval($row['ID']),
                'user' => $row['User'],
                'email' => $row['Email'],
                'nombre' => $row['Nombre'],
                'apellido' => $row['Apellido'],
                'genero' => $row['Genero'],
                'ubicacion' => $row['Ubicacion'],
                'foto' => $foto
            ]
        ];

        error_log("Enviando respuesta exitosa: " . json_encode($response));
        echo json_encode($response, JSON_UNESCAPED_UNICODE);
    } else {
        error_log("Usuario no encontrado: " . $user);
        http_response_code(404);
        echo json_encode([
            'success' => false,
            'error' => 'Usuario no encontrado'
        ]);
    }

} catch (Exception $e) {
    error_log("Error en get_profile.php: " . $e->getMessage());
    http_response_code(400);
    echo json_encode([
        'success' => false,
        'error' => $e->getMessage()
    ]);
} finally {
    // Cerrar recursos
    if (isset($stmt)) {
        $stmt->close();
    }
    if (isset($conn)) {
        $conn->close();
    }
}
?> 
