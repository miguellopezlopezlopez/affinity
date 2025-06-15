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
    // Obtener usuarios disponibles excluyendo al usuario actual y al admin
    $query = "
        SELECT DISTINCT u.ID, u.User, u.Email, u.Nombre, u.Apellido, u.Genero, u.Foto, u.Ubicacion,
        p.Biografia, p.Intereses, p.Preferencias, p.FotoPrincipal
        FROM Usuario u
        LEFT JOIN Perfil p ON u.ID = p.ID_User
        LEFT JOIN Matches m ON (u.ID = m.ID_Acept AND m.ID_Sol = (SELECT ID FROM Usuario WHERE User = ?))
            OR (u.ID = m.ID_Sol AND m.ID_Acept = (SELECT ID FROM Usuario WHERE User = ?))
        WHERE u.User != ? 
        AND u.ID != 1
        AND m.ID_Match IS NULL
    ";

    $stmt = $conn->prepare($query);
    $stmt->bind_param("sss", $currentUser, $currentUser, $currentUser);
    $stmt->execute();
    $result = $stmt->get_result();

    $users = [];
    while ($row = $result->fetch_assoc()) {
        // Construir URL completa para las fotos
        $fotoUrl = !empty($row['FotoPrincipal']) ? 
            "http://192.168.0.100/page/uploads/" . $row['FotoPrincipal'] : 
            "http://192.168.0.100/page/uploads/" . $row['Foto'];

        $users[] = [
            'id' => intval($row['ID']),
            'user' => $row['User'],
            'email' => $row['Email'],
            'nombre' => $row['Nombre'],
            'apellido' => $row['Apellido'],
            'genero' => $row['Genero'],
            'ubicacion' => $row['Ubicacion'],
            'foto' => $fotoUrl,
            'biografia' => $row['Biografia'],
            'intereses' => $row['Intereses'],
            'preferencias' => $row['Preferencias']
        ];
    }

    echo json_encode([
        'success' => true,
        'data' => $users
    ]);

} catch (Exception $e) {
    echo json_encode([
        'success' => false,
        'error' => 'Error al obtener usuarios: ' . $e->getMessage()
    ]);
} finally {
    if (isset($stmt)) {
        $stmt->close();
    }
    $conn->close();
}
?> 