<?php
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: http://localhost');
header('Access-Control-Allow-Methods: GET, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type');
header('Access-Control-Allow-Credentials: true');

session_start();

// Verificar si el usuario está autenticado
if (!isset($_SESSION['user_id'])) {
    http_response_code(401);
    echo json_encode(['success' => false, 'message' => 'No has iniciado sesión']);
    exit();
}

// Verificar que el usuario solo pueda ver sus propias estadísticas
$requestedUserId = isset($_GET['userId']) ? $_GET['userId'] : null;
if ($requestedUserId != $_SESSION['user_id']) {
    http_response_code(403);
    echo json_encode(['success' => false, 'message' => 'No tienes permiso para ver estas estadísticas']);
    exit();
}

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

if ($_SERVER['REQUEST_METHOD'] !== 'GET') {
    http_response_code(405);
    echo json_encode(['success' => false, 'message' => 'Método no permitido']);
    exit();
}

// Obtener el ID de usuario de la URL
$userId = isset($_GET['userId']) ? $_GET['userId'] : null;

if (!$userId) {
    http_response_code(400);
    echo json_encode(['success' => false, 'message' => 'ID de usuario requerido']);
    exit();
}

try {
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if($conn->connect_error) {
        throw new Exception('Error de conexión: ' . $conn->connect_error);
    }

    // Estadísticas básicas del usuario
    $stats = [
        'total_matches' => 0,
        'matches_activos' => 0,
        'mensajes_enviados' => 0,
        'mensajes_recibidos' => 0,
        'fecha_registro' => '',
        'ultimo_login' => ''
    ];

    // Obtener total de matches
    $query = "SELECT COUNT(*) as total FROM Matches WHERE (ID_Acept = ? OR ID_Sol = ?) AND Fecha_Aceptado IS NOT NULL";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("ii", $userId, $userId);
    $stmt->execute();
    $result = $stmt->get_result();
    if($row = $result->fetch_assoc()) {
        $stats['total_matches'] = $row['total'];
    }

    // Obtener matches activos (con mensajes en los últimos 30 días)
    $query = "SELECT COUNT(DISTINCT m.ID_Match) as activos 
              FROM Matches m 
              JOIN Mensaje msg ON m.ID_Match = msg.ID_Match 
              WHERE (m.ID_Acept = ? OR m.ID_Sol = ?) 
              AND m.Fecha_Aceptado IS NOT NULL 
              AND msg.Fecha_Hora >= DATE_SUB(NOW(), INTERVAL 30 DAY)";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("ii", $userId, $userId);
    $stmt->execute();
    $result = $stmt->get_result();
    if($row = $result->fetch_assoc()) {
        $stats['matches_activos'] = $row['activos'];
    }

    // Obtener total de mensajes enviados
    $query = "SELECT COUNT(*) as enviados FROM Mensaje WHERE ID_Emisor = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("i", $userId);
    $stmt->execute();
    $result = $stmt->get_result();
    if($row = $result->fetch_assoc()) {
        $stats['mensajes_enviados'] = $row['enviados'];
    }

    // Obtener total de mensajes recibidos
    $query = "SELECT COUNT(*) as recibidos FROM Mensaje WHERE ID_Receptor = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("i", $userId);
    $stmt->execute();
    $result = $stmt->get_result();
    if($row = $result->fetch_assoc()) {
        $stats['mensajes_recibidos'] = $row['recibidos'];
    }

    // Obtener información del usuario
    $query = "SELECT User, Nombre, Apellido, Genero FROM Usuario WHERE ID = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("i", $userId);
    $stmt->execute();
    $result = $stmt->get_result();
    if($row = $result->fetch_assoc()) {
        $stats['usuario'] = $row;
    }

    $conn->close();

    http_response_code(200);
    echo json_encode(['success' => true, 'data' => $stats]);

} catch (Exception $e) {
    http_response_code(500);
    echo json_encode(['success' => false, 'message' => 'Error en el servidor: ' . $e->getMessage()]);
} 