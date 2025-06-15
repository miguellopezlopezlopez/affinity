<?php
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET');
header('Access-Control-Allow-Headers: Content-Type');

require_once 'config.php';

if ($_SERVER['REQUEST_METHOD'] !== 'GET') {
    echo json_encode(['success' => false, 'error' => 'Método no permitido']);
    exit;
}

if (!isset($_GET['current_user'])) {
    echo json_encode(['success' => false, 'error' => 'Usuario no especificado']);
    exit;
}

$currentUser = $_GET['current_user'];

try {
    // Primero verificar que el usuario existe y obtener su ID
    $userQuery = "SELECT ID FROM Usuario WHERE User = ?";
    $stmt = $conn->prepare($userQuery);
    $stmt->bind_param("s", $currentUser);
    $stmt->execute();
    $userResult = $stmt->get_result();
    
    if ($userResult->num_rows === 0) {
        echo json_encode(['success' => false, 'error' => 'Usuario no encontrado']);
        exit;
    }
    
    $userId = $userResult->fetch_assoc()['ID'];

    // Obtener solicitudes de match pendientes donde el usuario actual es el receptor
    $query = "
        SELECT 
            m.ID_Match,
            m.Fecha_Solicitud,
            u.ID as solicitante_id,
            u.User as solicitante_user,
            u.Nombre as solicitante_nombre,
            u.Apellido as solicitante_apellido,
            u.Foto as solicitante_foto,
            p.FotoPrincipal as solicitante_foto_principal
        FROM Matches m
        INNER JOIN Usuario u ON m.ID_Sol = u.ID
        LEFT JOIN Perfil p ON u.ID = p.ID_User
        WHERE m.ID_Acept = ?
        AND m.Fecha_Aceptado IS NULL
        AND m.ID_Match > 0
    ";

    $stmt = $conn->prepare($query);
    $stmt->bind_param("i", $userId);
    $stmt->execute();
    $result = $stmt->get_result();

    $pendingMatches = [];
    while ($row = $result->fetch_assoc()) {
        // Validar que el ID del match sea válido
        $matchId = intval($row['ID_Match']);
        if ($matchId <= 0) {
            continue; // Saltar matches con IDs inválidos
        }

        // Construir URL completa para las fotos
        $fotoUrl = !empty($row['solicitante_foto_principal']) ? 
            "http://192.168.0.100/page/uploads/" . $row['solicitante_foto_principal'] : 
            "http://192.168.0.100/page/uploads/" . $row['solicitante_foto'];

        $solicitanteId = intval($row['solicitante_id']);
        if ($solicitanteId <= 0) {
            continue; // Saltar matches con IDs de solicitante inválidos
        }

        $pendingMatches[] = [
            'match_id' => $matchId,
            'fecha_solicitud' => $row['Fecha_Solicitud'],
            'solicitante' => [
                'id' => $solicitanteId,
                'user' => $row['solicitante_user'],
                'nombre' => $row['solicitante_nombre'],
                'apellido' => $row['solicitante_apellido'],
                'foto' => $fotoUrl
            ]
        ];
    }

    echo json_encode([
        'success' => true,
        'data' => $pendingMatches
    ]);

} catch (Exception $e) {
    echo json_encode([
        'success' => false,
        'error' => 'Error al obtener solicitudes pendientes: ' . $e->getMessage()
    ]);
} finally {
    if (isset($stmt)) {
        $stmt->close();
    }
    $conn->close();
}
?> 