<?php
// Configuración de errores
error_reporting(E_ALL);
ini_set('display_errors', 0);

// Configuración de la base de datos
define('DB_HOST', 'localhost');
define('DB_USER', 'root');
define('DB_PASS', '');
define('DB_NAME', 'filtring');

// Función para hashear contraseñas
function hash_password($password) {
    return hash('sha256', $password);
}

try {
    // Crear conexión
    $conn = new mysqli(DB_HOST, DB_USER, DB_PASS, DB_NAME);

    // Verificar conexión
    if ($conn->connect_error) {
        throw new Exception("Error de conexión: " . $conn->connect_error);
    }

    // Establecer charset
    if (!$conn->set_charset("utf8mb4")) {
        throw new Exception("Error cargando el conjunto de caracteres utf8mb4: " . $conn->error);
    }

    // Seleccionar la base de datos
    if (!$conn->select_db(DB_NAME)) {
        throw new Exception("Error seleccionando la base de datos: " . $conn->error);
    }

} catch (Exception $e) {
    // Log del error
    error_log("Error en config.php: " . $e->getMessage());
    
    // Si estamos en una solicitud API, devolver error JSON
    if (strpos($_SERVER['REQUEST_URI'], '/api/') !== false) {
        header('Content-Type: application/json; charset=utf-8');
        http_response_code(500);
        echo json_encode([
            'success' => false,
            'message' => 'Error de conexión a la base de datos'
        ]);
        exit();
    }
    // Si no es una solicitud API, lanzar la excepción
    throw $e;
}

// Función para sanitizar inputs
function sanitize_input($data) {
    global $conn;
    if (is_array($data)) {
        return array_map('sanitize_input', $data);
    }
    return $conn->real_escape_string(trim($data));
}

// Función para validar email
function is_valid_email($email) {
    return filter_var($email, FILTER_VALIDATE_EMAIL);
}

// Función para generar un token seguro
function generate_token($length = 32) {
    return bin2hex(random_bytes($length));
}

// Función para validar token
function validate_token($token) {
    return preg_match('/^[a-f0-9]{64}$/', $token);
}

// Función para registrar actividad
function log_activity($user_id, $action, $details = '') {
    global $conn;
    $stmt = $conn->prepare("INSERT INTO activity_log (user_id, action, details, ip_address) VALUES (?, ?, ?, ?)");
    $ip = $_SERVER['REMOTE_ADDR'];
    $stmt->bind_param("isss", $user_id, $action, $details, $ip);
    $stmt->execute();
    $stmt->close();
}
?> 