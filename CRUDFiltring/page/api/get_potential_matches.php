<?php
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET');
header('Access-Control-Allow-Headers: Content-Type');

require_once 'config.php';

if ($_SERVER['REQUEST_METHOD'] !== 'GET') {
    echo json_encode(['success' => false, 'message' => 'Only GET method is allowed']);
    exit;
}

if (!isset($_GET['userId'])) {
    echo json_encode(['success' => false, 'message' => 'User ID is required']);
    exit;
}

$userId = intval($_GET['userId']);

try {
    $conn = new mysqli($host, $user, $password, $database);

    if ($conn->connect_error) {
        throw new Exception("Connection failed: " . $conn->connect_error);
    }

    // Get users that:
    // 1. Are not the current user
    // 2. Are not already matched with the current user
    // 3. Have not been sent a match request by the current user
    $sql = "
        SELECT DISTINCT u.ID, u.User, u.Email, u.Nombre, u.Apellido, u.Genero, u.Foto, u.Ubicacion,
               p.Biografia, p.Intereses, p.Preferencias, p.FotoPrincipal
        FROM usuario u
        LEFT JOIN perfil p ON u.ID = p.ID_User
        WHERE u.ID != ? 
        AND u.ID != 1
        AND u.ID NOT IN (
            SELECT ID_Acept 
            FROM matches 
            WHERE ID_Sol = ?
            UNION
            SELECT ID_Sol 
            FROM matches 
            WHERE ID_Acept = ? AND Fecha_Aceptado IS NOT NULL
        )
        ORDER BY RAND()
        LIMIT 20
    ";

    $stmt = $conn->prepare($sql);
    $stmt->bind_param("iii", $userId, $userId, $userId);
    $stmt->execute();
    $result = $stmt->get_result();

    $profiles = [];
    while ($row = $result->fetch_assoc()) {
        // Use FotoPrincipal if available, otherwise use Foto
        $foto = !empty($row['FotoPrincipal']) ? $row['FotoPrincipal'] : $row['Foto'];
        
        $profiles[] = [
            'id' => $row['ID'],
            'user' => $row['User'],
            'email' => $row['Email'],
            'nombre' => $row['Nombre'],
            'apellido' => $row['Apellido'],
            'genero' => $row['Genero'],
            'ubicacion' => $row['Ubicacion'],
            'foto' => $foto,
            'biografia' => $row['Biografia'],
            'intereses' => $row['Intereses'],
            'preferencias' => $row['Preferencias']
        ];
    }

    echo json_encode([
        'success' => true,
        'data' => $profiles
    ]);

} catch (Exception $e) {
    echo json_encode([
        'success' => false,
        'message' => $e->getMessage()
    ]);
} finally {
    if (isset($conn)) {
        $conn->close();
    }
}
?> 