<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: POST, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type');
header('Access-Control-Allow-Credentials: true');

// Log de inicio
error_log("=== UPDATE_PROFILE.PHP INICIADO ===");

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

// Validar datos requeridos
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

    $userId = $conn->real_escape_string($data['userId']);
    $biografia = isset($data['biografia']) ? $conn->real_escape_string($data['biografia']) : '';
    $intereses = isset($data['intereses']) ? $conn->real_escape_string($data['intereses']) : '';
    $preferencias = isset($data['preferencias']) ? $conn->real_escape_string($data['preferencias']) : '';

    error_log("Datos procesados - userId: $userId, biografia: $biografia, intereses: $intereses, preferencias: $preferencias");

    // Verificar si existe un perfil para el usuario
    $checkQuery = "SELECT ID_Perfil FROM Perfil WHERE ID_User = ?";
    $checkStmt = $conn->prepare($checkQuery);
    $checkStmt->bind_param("i", $userId);
    $checkStmt->execute();
    $result = $checkStmt->get_result();

    if ($result->num_rows > 0) {
        // Actualizar perfil existente
        error_log("Actualizando perfil existente");
        $query = "UPDATE Perfil SET Biografia = ?, Intereses = ?, Preferencias = ? WHERE ID_User = ?";
        $stmt = $conn->prepare($query);
        $stmt->bind_param("sssi", $biografia, $intereses, $preferencias, $userId);
    } else {
        // Crear nuevo perfil
        error_log("Creando nuevo perfil");
        $query = "INSERT INTO Perfil (ID_User, Biografia, Intereses, Preferencias) VALUES (?, ?, ?, ?)";
        $stmt = $conn->prepare($query);
        $stmt->bind_param("isss", $userId, $biografia, $intereses, $preferencias);
    }

    if ($stmt->execute()) {
        error_log("Perfil actualizado correctamente");
        echo json_encode([
            'success' => true,
            'message' => 'Perfil actualizado correctamente'
        ]);
    } else {
        error_log("Error al actualizar el perfil: " . $stmt->error);
        throw new Exception('Error al actualizar el perfil');
    }

} catch (Exception $e) {
    error_log("Error en update_profile.php: " . $e->getMessage());
    http_response_code(500);
    echo json_encode([
        'success' => false,
        'message' => $e->getMessage()
    ]);
} finally {
    if (isset($conn)) {
        $conn->close();
    }
    error_log("=== UPDATE_PROFILE.PHP TERMINADO ===");
} 