<?php
require_once 'utils/session.php';

header('Access-Control-Allow-Origin: *');
header('Content-Type: application/json; charset=UTF-8');
header('Access-Control-Allow-Methods: GET, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');

if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    http_response_code(200);
    exit();
}

if ($_SERVER['REQUEST_METHOD'] !== 'GET') {
    http_response_code(405);
    echo json_encode(['success' => false, 'message' => 'Método no permitido']);
    exit();
}

// Verificar autenticación
$currentUserId = getCurrentUserId();
if (!$currentUserId) {
    http_response_code(401);
    echo json_encode(['success' => false, 'message' => 'No autenticado']);
    exit();
}

// Obtener el ID del usuario del perfil a mostrar
$profileUserId = isset($_GET['userId']) ? $_GET['userId'] : $currentUserId;

try {
    $conn = new mysqli('localhost', 'root', '', 'filtring');

    if ($conn->connect_error) {
        throw new Exception('Error de conexión: ' . $conn->connect_error);
    }

    $userId = $conn->real_escape_string($profileUserId);

    // Obtener datos básicos del usuario
    $query = "SELECT u.User, u.Nombre, u.Apellido, u.Ubicacion, 
                     p.Biografia, p.Intereses, p.Preferencias, p.FotoPrincipal
              FROM Usuario u
              LEFT JOIN Perfil p ON u.ID = p.ID_User
              WHERE u.ID = ?";

    $stmt = $conn->prepare($query);
    $stmt->bind_param("i", $userId);
    $stmt->execute();
    $result = $stmt->get_result();
    $userData = $result->fetch_assoc();

    if (!$userData) {
        throw new Exception('Usuario no encontrado');
    }

    // Obtener fotos adicionales
    $queryFotos = "SELECT ID, URL FROM FotosPerfil WHERE ID_User = ? AND Tipo = 'galeria' ORDER BY ID";
    $stmtFotos = $conn->prepare($queryFotos);
    $stmtFotos->bind_param("i", $userId);
    $stmtFotos->execute();
    $resultFotos = $stmtFotos->get_result();
    
    $fotos = [];
    while ($foto = $resultFotos->fetch_assoc()) {
        $fotos[] = [
            'id' => $foto['ID'],
            'url' => $foto['URL']
        ];
    }

    $response = [
        'success' => true,
        'user' => $userData['User'],
        'nombre' => $userData['Nombre'],
        'apellido' => $userData['Apellido'],
        'ubicacion' => $userData['Ubicacion'],
        'biografia' => $userData['Biografia'],
        'intereses' => $userData['Intereses'],
        'preferencias' => $userData['Preferencias'],
        'foto_principal' => $userData['FotoPrincipal'],
        'fotos' => $fotos,
        'isOwnProfile' => $userId == $currentUserId
    ];

    echo json_encode($response);

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