<?php
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: POST');
header('Access-Control-Allow-Headers: Content-Type');

require_once 'config.php';

if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    echo json_encode(['success' => false, 'error' => 'Método no permitido']);
    exit;
}

if (!isset($_GET['match_id'])) {
    echo json_encode(['success' => false, 'error' => 'ID de match no especificado']);
    exit;
}

$matchId = intval($_GET['match_id']);

try {
    // Verificar que el match existe y no ha sido aceptado aún
    $checkQuery = "SELECT * FROM Matches WHERE ID_Match = ? AND Fecha_Aceptado IS NULL";
    $stmt = $conn->prepare($checkQuery);
    $stmt->bind_param("i", $matchId);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows === 0) {
        echo json_encode(['success' => false, 'error' => 'Match no encontrado o ya aceptado']);
        exit;
    }

    // Eliminar el match
    $deleteQuery = "DELETE FROM Matches WHERE ID_Match = ?";
    $stmt = $conn->prepare($deleteQuery);
    $stmt->bind_param("i", $matchId);
    
    if ($stmt->execute()) {
        echo json_encode([
            'success' => true,
            'message' => 'Match rechazado exitosamente'
        ]);
    } else {
        throw new Exception("Error al eliminar el match");
    }

} catch (Exception $e) {
    echo json_encode([
        'success' => false,
        'error' => 'Error al rechazar match: ' . $e->getMessage()
    ]);
} finally {
    if (isset($stmt)) {
        $stmt->close();
    }
    $conn->close();
}
?>