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

if (!isset($input['photoId']) || !isset($input['userId'])) {
    http_response_code(400);
    echo json_encode(['success' => false, 'message' => 'Faltan datos requeridos']);
    exit();
}

try {
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if ($conn->connect_error) {
        throw new Exception('Error de conexión: ' . $conn->connect_error);
    }

    $photoId = $conn->real_escape_string($input['photoId']);
    $userId = $conn->real_escape_string($input['userId']);

    // Obtener la URL de la foto antes de eliminarla
    $queryUrl = "SELECT URL FROM FotosPerfil WHERE ID = ? AND ID_User = ?";
    $stmtUrl = $conn->prepare($queryUrl);
    $stmtUrl->bind_param("ii", $photoId, $userId);
    $stmtUrl->execute();
    $result = $stmtUrl->get_result();
    $foto = $result->fetch_assoc();

    if (!$foto) {
        throw new Exception('Foto no encontrada o no tienes permiso para eliminarla');
    }

    // Eliminar el registro de la base de datos
    $query = "DELETE FROM FotosPerfil WHERE ID = ? AND ID_User = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("ii", $photoId, $userId);
    $stmt->execute();

    if ($stmt->affected_rows > 0) {
        // Eliminar el archivo físico
        $filePath = '../' . $foto['URL'];
        if (file_exists($filePath)) {
            unlink($filePath);
        }

        echo json_encode([
            'success' => true,
            'message' => 'Foto eliminada correctamente'
        ]);
    } else {
        throw new Exception('No se pudo eliminar la foto');
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