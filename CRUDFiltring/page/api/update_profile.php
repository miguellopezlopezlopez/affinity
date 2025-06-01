<?php
header('Access-Control-Allow-Origin: *');
header('Content-Type: application/json; charset=UTF-8');
header('Access-Control-Allow-Methods: POST, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    http_response_code(405);
    echo json_encode(['success' => false, 'message' => 'Método no permitido']);
    exit();
}

$input = json_decode(file_get_contents('php://input'), true);

if (!isset($input['userId'])) {
    http_response_code(400);
    echo json_encode(['success' => false, 'message' => 'ID de usuario no proporcionado']);
    exit();
}

try {
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if ($conn->connect_error) {
        throw new Exception('Error de conexión: ' . $conn->connect_error);
    }

    $userId = $conn->real_escape_string($input['userId']);
    $biografia = $conn->real_escape_string($input['biografia'] ?? '');
    $intereses = $conn->real_escape_string($input['intereses'] ?? '');
    $preferencias = $conn->real_escape_string($input['preferencias'] ?? '');

    // Actualizar o insertar en la tabla Perfil
    $query = "INSERT INTO Perfil (ID_User, Biografia, Intereses, Preferencias)
              VALUES (?, ?, ?, ?)
              ON DUPLICATE KEY UPDATE
              Biografia = VALUES(Biografia),
              Intereses = VALUES(Intereses),
              Preferencias = VALUES(Preferencias)";

    $stmt = $conn->prepare($query);
    $stmt->bind_param("isss", $userId, $biografia, $intereses, $preferencias);
    $stmt->execute();

    if ($stmt->affected_rows > 0) {
        echo json_encode(['success' => true, 'message' => 'Perfil actualizado correctamente']);
    } else {
        echo json_encode(['success' => true, 'message' => 'No se realizaron cambios']);
    }

} catch (Exception $e) {
    http_response_code(500);
    echo json_encode([
        'success' => false,
        'message' => $e->getMessage()
    ]);
} finally {
    if (isset($conn)) {
        $conn->close();
    }
} 