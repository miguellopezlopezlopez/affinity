<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: POST, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type');
header('Access-Control-Allow-Credentials: true');

// Log de inicio
error_log("=== DELETE_USER.PHP INICIADO ===");

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    error_log("Método no permitido: " . $_SERVER['REQUEST_METHOD']);
    http_response_code(405);
    echo json_encode(['success' => false, 'message' => 'Método no permitido']);
    exit();
}

// Obtener datos del cuerpo de la solicitud
$data = json_decode(file_get_contents('php://input'), true);
error_log("Datos recibidos: " . print_r($data, true));

// Validar que se proporcionó el ID
if (!isset($data['userId'])) {
    error_log("ID de usuario no proporcionado");
    http_response_code(400);
    echo json_encode(['success' => false, 'message' => 'ID de usuario no proporcionado']);
    exit();
}

try {
    error_log("Conectando a la base de datos...");
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if ($conn->connect_error) {
        error_log("Error de conexión: " . $conn->connect_error);
        throw new Exception('Error de conexión a la base de datos');
    }

    // Escapar el ID de usuario
    $userId = $conn->real_escape_string($data['userId']);

    // Primero verificar si el usuario existe
    $checkUser = $conn->prepare("SELECT ID FROM usuario WHERE ID = ?");
    $checkUser->bind_param("i", $userId);
    $checkUser->execute();
    
    if ($checkUser->get_result()->num_rows === 0) {
        throw new Exception('Usuario no encontrado');
    }
    $checkUser->close();

    // Eliminar el usuario - las demás tablas se limpiarán automáticamente por el ON DELETE CASCADE
    $stmt = $conn->prepare("DELETE FROM usuario WHERE ID = ?");
    $stmt->bind_param("i", $userId);
    
    if (!$stmt->execute()) {
        throw new Exception('Error al eliminar el usuario');
    }
    
    error_log("Usuario eliminado correctamente");
    echo json_encode([
        'success' => true,
        'message' => 'Usuario eliminado correctamente'
    ]);

} catch (Exception $e) {
    error_log("Error en delete_user.php: " . $e->getMessage());
    http_response_code(500);
    echo json_encode([
        'success' => false,
        'message' => $e->getMessage()
    ]);
} finally {
    if (isset($stmt)) {
        $stmt->close();
    }
    if (isset($conn)) {
        $conn->close();
    }
    error_log("=== DELETE_USER.PHP TERMINADO ===");
}